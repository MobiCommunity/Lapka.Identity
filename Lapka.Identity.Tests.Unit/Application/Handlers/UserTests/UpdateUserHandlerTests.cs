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
    public class UpdateUserHandlerTests
    {
        private readonly UpdateUserHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IEventProcessor _eventProcessor;

        public UpdateUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdateUserHandler(_eventProcessor, _userRepository);
        }

        private Task Act(UpdateUser command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_user_should_update()
        {
            User arrangeUser = Extensions.ArrangeUser();

            User user = User.Create(arrangeUser.Id.Value, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, arrangeUser.CreatedAt, arrangeUser.Role);

            UpdateUser command = new UpdateUser(user.Id.Value, "newUsername", "firstName", "lastName");
            _userRepository.GetAsync(command.Id).Returns(user);

            await Act(command);

            await _userRepository.Received().UpdateAsync(user);
            await _eventProcessor.Received().ProcessAsync(user.Events);
        }
    }
}