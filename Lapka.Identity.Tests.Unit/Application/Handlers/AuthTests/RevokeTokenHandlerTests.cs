using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Commands.Handlers.Auth;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Core.Entities;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.AuthTests
{
    public class RevokeTokenHandlerTests
    {
        private readonly RevokeTokenHandler _handler;
        private readonly IRefreshTokenService _refreshTokenService;

        public RevokeTokenHandlerTests()
        {

            _refreshTokenService = Substitute.For<IRefreshTokenService>();
            _handler = new RevokeTokenHandler(_refreshTokenService);
        }

        private Task Act(RevokeRefreshToken command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_credentials_should_sign_in()
        {
            RevokeRefreshToken command = new RevokeRefreshToken("tokenToRevoke");

            await Act(command);

            await _refreshTokenService.Received().RevokeAsync(command.Token);
        }
    }
}