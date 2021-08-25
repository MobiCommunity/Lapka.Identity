using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services.Auth;

namespace Lapka.Identity.Application.Commands.Handlers.Auth
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