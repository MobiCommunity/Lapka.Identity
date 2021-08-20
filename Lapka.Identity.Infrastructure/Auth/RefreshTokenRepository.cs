using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Auth
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<JsonWebTokenDocument, Guid> _refreshTokens;

        public RefreshTokenRepository(IMongoRepository<JsonWebTokenDocument, Guid> refreshTokens)
        {
            _refreshTokens = refreshTokens;
        }
        public async Task<RefreshToken> GetAsync(string token)
        {
            JsonWebTokenDocument refreshToken = await _refreshTokens.
                GetAsync(x => x.RefreshToken == token);

            return refreshToken?.AsBusiness();
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _refreshTokens.AddAsync(new JsonWebTokenDocument
            {
                Id = token.Id.Value,
                RefreshToken = token.Token,
                UserId = token.UserId,
                ExpiresAt = token.RevokedAt
            });
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            await _refreshTokens.UpdateAsync(token.AsDocument());
        }
    }
}