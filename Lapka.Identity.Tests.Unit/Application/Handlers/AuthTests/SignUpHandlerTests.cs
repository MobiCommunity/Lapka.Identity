using System;
using System.Threading.Tasks;
using Lapka.Identity.Api.Models;
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
    public class SignUpHandlerTests
    {
        private readonly SignUpHandler _handler;
        private readonly IIdentityService _identityService;

        public SignUpHandlerTests()
        {

            _identityService = Substitute.For<IIdentityService>();
            _handler = new SignUpHandler(_identityService);
        }

        private Task Act(SignUp command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_user_should_sign_up()
        {
            Guid id = Guid.NewGuid();
            DateTime createdAt = DateTime.UtcNow;
            User arrangeUser = Extensions.ArrangeUser();
            UserAuth userAuth = Extensions.ArrangeUserAuth();
            
            SignUp command = new SignUp(id, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, createdAt, userAuth.Role);

            await Act(command);

            await _identityService.Received().SignUpAsync(Arg.Is(command));
        }
    }
}