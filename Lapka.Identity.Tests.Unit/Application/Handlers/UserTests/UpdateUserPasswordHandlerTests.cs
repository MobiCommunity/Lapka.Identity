using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.UserTests
{
    public class UpdateUserPasswordHandlerTests
    {
        private readonly UpdateUserPasswordHandler _handler;
        private readonly IIdentityService _identityService;

        public UpdateUserPasswordHandlerTests()
        {
            _identityService = Substitute.For<IIdentityService>();
            _handler = new UpdateUserPasswordHandler(_identityService);
        }

        private Task Act(UpdateUserPassword command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_user_should_update()
        {
            User arrangeUser = Extensions.ArrangeUser();

            User user = User.Create(arrangeUser.Id.Value, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, arrangeUser.CreatedAt, arrangeUser.Role);

            UpdateUserPassword command = new UpdateUserPassword(user.Id.Value, "newPassword");

            await Act(command);

            await _identityService.Received().ChangeUserPasswordAsync(command);
        }
    }
}