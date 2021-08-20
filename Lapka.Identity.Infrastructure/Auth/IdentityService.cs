using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Identity;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Infrastructure.Auth
{
    public class IdentityService : IIdentityService
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IRefreshTokenService refreshTokenService, IGrpcPhotoService grpcPhotoService,
            ILogger<IdentityService> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _refreshTokenService = refreshTokenService;
            _grpcPhotoService = grpcPhotoService;
            _logger = logger;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            User user = await _userRepository.GetAsync(id);

            return user is null ? null : new UserDto(user);
        }

        public async Task<AuthDto> SignInAsync(SignIn command)
        {
            if (!EmailRegex.IsMatch(command.Email))
            {
                throw new InvalidEmailValueException(command.Email);
            }

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
            if (!EmailRegex.IsMatch(command.Email))
            {
                throw new InvalidEmailValueException(command.Email);
            }

            User user = await _userRepository.GetAsync(command.Email);
            if (user is {})
            {
                throw new EmailInUseException(command.Email);
            }
            
            string role = "user";
            string password = _passwordService.Hash(command.Password);
            user = new User(command.Id, command.Username, command.FirstName, command.LastName, command.Email, 
                password, null, null, command.CreatedAt, role);

            await _userRepository.AddAsync(user);
        }
    }
}