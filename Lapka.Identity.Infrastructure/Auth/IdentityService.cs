using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Identity;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Core.Exceptions.Token;
using Lapka.Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Infrastructure.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IFacebookAuthHelper _facebookAuthHelper;
        private readonly IGoogleAuthHelper _googleAuthHelper;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService, IFacebookAuthHelper facebookAuthHelper)
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService, IGoogleAuthHelper googleAuthHelper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _refreshTokenService = refreshTokenService;
            _facebookAuthHelper = facebookAuthHelper;
            _googleAuthHelper = googleAuthHelper;
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
            GoogleUser googleUser = await _googleAuthHelper.GetUserInfoAsync(command.AccessToken);
            User user = await _userRepository.GetAsync(googleUser.Email);
            
            if (user is null)
            {
                await SignUpAsync(new SignUp(Guid.NewGuid(), googleUser.Email, googleUser.GivenName,
                    googleUser.FamilyName ?? "", googleUser.Email, Guid.NewGuid().ToString(), DateTime.Now));
            }
            else
            {
                user.Update(googleUser.Email, googleUser.GivenName, googleUser.FamilyName, googleUser.Email,
                    user.PhoneNumber, user.Role, googleUser.Picture);
                await _userRepository.UpdateAsync(user);
            }
            user = await _userRepository.GetAsync(googleUser.Email);

            AuthDto auth = await GetTokensAsync(user);

            return auth;
        }

        public async Task SignUpAsync(SignUp command)
        {
            User user = await _userRepository.GetAsync(command.Email);
            if (user is {})
            {
                throw new EmailInUseException(command.Email);
            }

            string role = "user";
            string password = _passwordService.Hash(command.Password);
            user = User.Create(command.Id, command.Username, command.FirstName, command.LastName, command.Email,
                password, command.CreatedAt, role);

            await _userRepository.AddAsync(user);
        }

        public async Task<AuthDto> FacebookLoginAsync(SignInFacebook command)
        {
            var validatedTokenResult = await _facebookAuthHelper.ValidateAccessTokenAsync(command.AccessToken);

            if (!validatedTokenResult.Data.IsValid)
            {
                throw new InvalidAccessTokenException(command.AccessToken);
            }

            FacebookUserInfoResult userInfo = await _facebookAuthHelper.GetUserInfoAsync(command.AccessToken);

            User user = await _userRepository.GetAsync(userInfo.Email);

            if (user is null)
            {
                await SignUpAsync(new SignUp(Guid.NewGuid(), userInfo.Email, userInfo.FirstName,
                    userInfo.LastName, userInfo.Email, Guid.NewGuid().ToString(), DateTime.Now));
            }
            else
            {
                user.Update(userInfo.Email, userInfo.FirstName,
                    userInfo.LastName, userInfo.Email, user.PhoneNumber, user.Role, 
                    userInfo.FacebookPicture.Data.Url.AbsoluteUri);

               await _userRepository.UpdateAsync(user);
            }

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
            Dictionary<string, IEnumerable<string>> claims = new Dictionary<string, IEnumerable<string>>
            {
                [ClaimTypes.NameIdentifier] = new[] {user.Id.Value.ToString()}
            };

            AuthDto auth = _jwtProvider.Create(user.Id.Value, user.Role, claims: claims);
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id.Value);
            return auth;
        }
    }
}