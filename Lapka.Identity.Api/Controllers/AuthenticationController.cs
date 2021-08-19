using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/tokens")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthenticationController(ICommandDispatcher commandDispatcher, IRefreshTokenService refreshTokenService)
        {
            _commandDispatcher = commandDispatcher;
            _refreshTokenService = refreshTokenService;
        }
        
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest refreshToken)
        {
            await _commandDispatcher.SendAsync(new RevokeRefreshToken(refreshToken.Token));

            return NoContent();
        }
        
        [HttpPost("use")]
        public async Task<IActionResult> RevokeToken(RefreshTokenRequest refreshToken)
        {
            AuthDto token = await _refreshTokenService.UseAsync(refreshToken.Token);

            return Ok(token);
        }
    }
}