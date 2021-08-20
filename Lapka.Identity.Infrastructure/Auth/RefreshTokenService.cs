using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions.Token;
using Lapka.Identity.Core.Exceptions.User;

namespace Lapka.Identity.Infrastructure.Auth
{
   public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRng _rng;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository, IJwtProvider jwtProvider, IRng rng)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _rng = rng;
        }

        public async Task<string> CreateAsync(Guid userId)
        {
            string token = _rng.Generate(30, true);
            RefreshToken refreshToken = new RefreshToken(Guid.NewGuid(), userId, token, DateTime.UtcNow);
            await _refreshTokenRepository.AddAsync(refreshToken);

            return token;
        }

        public async Task RevokeAsync(string refreshToken)
        {
            RefreshToken token = await _refreshTokenRepository.GetAsync(refreshToken);
            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            token.Revoke(DateTime.UtcNow);
            await _refreshTokenRepository.UpdateAsync(token);
        }

        public async Task<AuthDto> UseAsync(string refreshToken)
        {
            RefreshToken token = await _refreshTokenRepository.GetAsync(refreshToken);
            if (token is null)
            {
                throw new InvalidRefreshTokenException();
            }

            if (token.Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            User user = await _userRepository.GetAsync(token.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(token.UserId.ToString());
            }

            Dictionary<string, IEnumerable<string>> claims = new Dictionary<string, IEnumerable<string>>
            {
                
            };
            AuthDto auth = _jwtProvider.Create(token.UserId, user.Role, claims: claims);
            auth.RefreshToken = refreshToken;

            return auth;
        }
    }
}