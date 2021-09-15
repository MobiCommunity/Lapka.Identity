using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Auths;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Identity;
using Lapka.Identity.Core.Exceptions.User;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Infrastructure.Auths
{
    public class IdentityService : IIdentityService
    {
        private const string BasicRole = "user";
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IFacebookAuthenticator _facebookAuthenticator;
        private readonly IGoogleAuthenticator _googleAuthenticator;
        private readonly IGrpcPhotoService _photoService;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService, IFacebookAuthenticator facebookAuthenticator,
            IGoogleAuthenticator googleAuthenticator, IGrpcPhotoService photoService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _refreshTokenService = refreshTokenService;
            _facebookAuthenticator = facebookAuthenticator;
            _googleAuthenticator = googleAuthenticator;
            _photoService = photoService;
        }

        public async Task<AuthDto> SignInAsync(SignIn command)
        {
            User user = await _userRepository.GetAsync(command.Email);
            if (user is null || !_passwordService.IsValid(user.Password, command.Password))
            {
                throw new InvalidCredentialsException(command.Email);
            }

            if (!_passwordService.IsValid(user.Password, command.Password))
            {
                throw new InvalidCredentialsException(command.Email);
            }
            
            AuthDto auth = await GetTokensAsync(user);

            return auth;
        }

        public async Task<AuthDto> SignInByGoogleAsync(SignInGoogle command)
        {
            GoogleUser googleUser = await _googleAuthenticator.GetUserInfoAsync(command.AccessToken);
            User user = await _userRepository.GetAsync(googleUser.Email);

            if (user is null)
            {
                await SignUpAsync(new SignUp(Guid.NewGuid(), googleUser.Email, googleUser.GivenName,
                    googleUser.FamilyName ?? "", googleUser.Email, Guid.NewGuid().ToString(), DateTime.Now, BasicRole));
                user = await _userRepository.GetAsync(googleUser.Email);
            }
            else
            {
                user.Update(user.Email, googleUser.GivenName, googleUser.FamilyName, user.PhoneNumber);
            }
            
            await CheckIfPhotoUpdated(user, googleUser.Picture);
            await _userRepository.UpdateAsync(user);

            user = await _userRepository.GetAsync(googleUser.Email);

            AuthDto auth = await GetTokensAsync(user);

            return auth;
        }

        public async Task SignUpAsync(SignUp command)
        {
            User user = await _userRepository.GetAsync(command.Email);
            if (user is { })
            {
                throw new EmailInUseException(command.Email);
            }

            if (command.Password.Length < MinimumPasswordLength)
            {
                throw new TooShortPasswordException();
            }
            
            string password = _passwordService.Hash(command.Password);
            user = User.Create(command.Id, command.Username, command.FirstName, command.LastName, command.Email,
                password, command.CreatedAt, command.Role);

            await _userRepository.AddAsync(user);
        }

        public async Task<AuthDto> FacebookLoginAsync(SignInFacebook command)
        {
            FacebookTokenValidationResult validatedTokenResult =
                await _facebookAuthenticator.ValidateAccessTokenAsync(command.AccessToken);

            if (!validatedTokenResult.Data.IsValid)
            {
                throw new InvalidAccessTokenException(command.AccessToken);
            }

            FacebookUserInfoResult userInfo = await _facebookAuthenticator.GetUserInfoAsync(command.AccessToken);

            User user = await _userRepository.GetAsync(userInfo.Email);

            if (user is null)
            {
                await SignUpAsync(new SignUp(Guid.NewGuid(), userInfo.Email, userInfo.FirstName,
                    userInfo.LastName, userInfo.Email, Guid.NewGuid().ToString(), DateTime.Now, BasicRole));
                user = await _userRepository.GetAsync(userInfo.Email);
            }
            else
            {
                user.Update(user.Email, userInfo.FirstName, userInfo.LastName, user.PhoneNumber);
            }
            
            await CheckIfPhotoUpdated(user, userInfo.FacebookPicture.Data.Url.AbsoluteUri);
            await _userRepository.UpdateAsync(user);

            user = await _userRepository.GetAsync(userInfo.Email);

            AuthDto auth = await GetTokensAsync(user);

            return auth;
        }

        public async Task ChangeUserPasswordAsync(UpdateUserPassword command)
        {
            User user = await _userRepository.GetAsync(command.Id);
            if (user is null)
            {
                throw new UserNotFoundException(command.Id.ToString());
            }

            string password = _passwordService.Hash(command.Password);

            user.UpdatePassword(password);

            await _userRepository.UpdateAsync(user);
        }

        private async Task<AuthDto> GetTokensAsync(User user)
        {
            AuthDto auth = _jwtProvider.Create(user.Id.Value, user.Role, claims: new Dictionary<string, IEnumerable<string>>());
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id.Value);
            return auth;
        }
        
        private async Task CheckIfPhotoUpdated(User user, string newPath)
        {
            if (user.PhotoId == Guid.Empty)
            {
                await SetNewPhotoAsync(user, "", newPath);
                return;
            }

            string photoPath = await _photoService.GetPhotoPathAsync(user.PhotoId, BucketName.UserPhotos);

            if (photoPath != newPath)
            {
                await SetNewPhotoAsync(user, photoPath, newPath);
            }
        }

        private async Task SetNewPhotoAsync(User user, string oldPath, string newPath)
        {
            Guid photoId = Guid.NewGuid();
            await _photoService.SetExternalPhotoAsync(photoId, oldPath, newPath,
                BucketName.UserPhotos);
            user.UpdatePhoto(photoId);
        }
        
        private const int MinimumPasswordLength = 5;
    }
}