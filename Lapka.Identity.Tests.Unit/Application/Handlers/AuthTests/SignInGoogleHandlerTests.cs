using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers.Auth;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.AuthTests
{
    public class SignInGoogleHandlerTests
    {
        private readonly SignInGoogleHandler _handler;
        private readonly IIdentityService _identityService;

        public SignInGoogleHandlerTests()
        {

            _identityService = Substitute.For<IIdentityService>();
            _handler = new SignInGoogleHandler(_identityService);
        }

        private Task Act(SignInGoogle command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_token_should_sign_in()
        {
            SignInGoogle command = new SignInGoogle("accessToken");

            await Act(command);

            await _identityService.Received().SignInByGoogleAsync(command);
        }
    }
}