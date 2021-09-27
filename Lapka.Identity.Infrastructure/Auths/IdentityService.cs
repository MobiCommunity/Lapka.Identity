using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Auths;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
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
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IFacebookAuthenticator _facebookAuthenticator;
        private readonly IGoogleAuthenticator _googleAuthenticator;

        public IdentityService(IEventProcessor eventProcessor, IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService,
            IFacebookAuthenticator facebookAuthenticator, IGoogleAuthenticator googleAuthenticator)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _refreshTokenService = refreshTokenService;
            _facebookAuthenticator = facebookAuthenticator;
            _googleAuthenticator = googleAuthenticator;
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
                    googleUser.FamilyName ?? "", googleUser.Email, Guid.NewGuid().ToString(),
                    DateTime.UtcNow, BasicRole, googleUser.Picture));

                user = await _userRepository.GetAsync(googleUser.Email);
            }
            else
            {
                user.Update(user.Email.Value, googleUser.GivenName, googleUser.FamilyName, user.PhoneNumber);
                user.UpdatePhoto(googleUser.Picture, "");
            }

            await _userRepository.UpdateAsync(user);

            user = await _userRepository.GetAsync(googleUser.Email);
            await _eventProcessor.ProcessAsync(user.Events);
            
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
            user = User.Create(command.Id, command.Username, command.FirstName, command.LastName,
                new EmailAddress(command.Email), password, command.CreatedAt, command.Role);

            await _userRepository.AddAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
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
                await SignUpAsync(new SignUp(Guid.NewGuid(), userInfo.Email, userInfo.FirstName, userInfo.LastName,
                    userInfo.Email, Guid.NewGuid().ToString(), DateTime.UtcNow, BasicRole,
                    userInfo.FacebookPicture.Data.Url.AbsoluteUri));

                user = await _userRepository.GetAsync(userInfo.Email);
            }
            else
            {
                user.Update(user.Email.Value, userInfo.FirstName, userInfo.LastName, user.PhoneNumber);
                user.UpdatePhoto(userInfo.FacebookPicture.Data.Url.AbsoluteUri, "");
            }

            await _userRepository.UpdateAsync(user);

            user = await _userRepository.GetAsync(userInfo.Email);
            await _eventProcessor.ProcessAsync(user.Events);

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
            AuthDto auth = _jwtProvider.Create(user.Id.Value, user.Role,
                claims: new Dictionary<string, IEnumerable<string>>());
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id.Value);
            return auth;
        }

        private const int MinimumPasswordLength = 5;
    }
}