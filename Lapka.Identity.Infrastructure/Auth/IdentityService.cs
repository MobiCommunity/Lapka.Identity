using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions.Identity;
using Lapka.Identity.Core.Exceptions.User;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _refreshTokenService = refreshTokenService;
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

            Dictionary<string, IEnumerable<string>> claims = new Dictionary<string, IEnumerable<string>>
            {
                [ClaimTypes.NameIdentifier] = new []{user.Id.Value.ToString()} 
            };
            
            AuthDto auth = _jwtProvider.Create(user.Id.Value, user.Role, claims: claims);
            auth.RefreshToken = await _refreshTokenService.CreateAsync(user.Id.Value);
            
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
                password,  command.CreatedAt, role);

            await _userRepository.AddAsync(user);
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
    }
}