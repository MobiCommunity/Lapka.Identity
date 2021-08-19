using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class RevokeTokenHandler : ICommandHandler<RevokeRefreshToken>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RevokeTokenHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }
        
        public async Task HandleAsync(RevokeRefreshToken command)
        {
            await _refreshTokenService.RevokeAsync(command.Token);
        }
    }
}