using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers.Auth;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers
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
            DateTime createdAt = DateTime.Now;
            User arrangeUser = Extensions.ArrangeUser();
            
            SignUp command = new SignUp(id, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, createdAt);

            await Act(command);

            await _identityService.Received().SignUpAsync(Arg.Is(command));
        }
    }
}