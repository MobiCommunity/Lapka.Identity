using System;
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
    public class SignInHandlerTests
    {
        private readonly SignInHandler _handler;
        private readonly IIdentityService _identityService;

        public SignInHandlerTests()
        {

            _identityService = Substitute.For<IIdentityService>();
            _handler = new SignInHandler(_identityService);
        }

        private Task Act(SignIn command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_credentials_should_sign_in()
        {
            User arrangeUser = Extensions.ArrangeUser();
            
            SignIn command = new SignIn(arrangeUser.Username, arrangeUser.Password);

            await Act(command);

            await _identityService.Received().SignInAsync(command);
        }
    }
}