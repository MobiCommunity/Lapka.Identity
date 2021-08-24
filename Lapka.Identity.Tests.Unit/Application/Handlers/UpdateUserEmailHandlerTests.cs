using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers
{
    public class UpdateUserEmailHandlerTests
    {
        private readonly UpdateUserEmailHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IEventProcessor _eventProcessor;

        public UpdateUserEmailHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdateUserEmailHandler(_eventProcessor, _userRepository);
        }

        private Task Act(UpdateUserEmail command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_user_should_update()
        {
            User arrangeUser = ArrangeUser();

            User user = User.Create(arrangeUser.Id.Value, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, arrangeUser.CreatedAt, arrangeUser.Role);

            UpdateUserEmail command = new UpdateUserEmail(user.Id.Value, "thisismynewemail@mail.com");
            _userRepository.GetAsync(command.Id).Returns(user);

            await Act(command);

            await _userRepository.Received()
                .UpdateAsync(Arg.Is(user));

            await _eventProcessor.Received().ProcessAsync(user.Events);
        }

        private User ArrangeUser(AggregateId id = null, string username = null, string firstName = null,
            string lastName = null, string email = null, string password = null, DateTime createdAt = default,
            string role = null)
        {
            AggregateId validId = id ?? new AggregateId();
            string validUsername = username ?? "Pomidorowy";
            string validFirstName = firstName ?? "Jasiek";
            string validLastName = lastName ?? "Skronowski";
            string validEmail = email ?? "Skronowski@email.com";
            string validPassword = password ?? "Secretpassword";
            string validRole = role ?? "user";
            DateTime validCreatedAt = DateTime.Now;
            if (createdAt != default)
            {
                validCreatedAt = createdAt;
            }

            User user = new User(validId.Value, validUsername, validFirstName, validLastName, validEmail, validPassword,
                null, null, validCreatedAt, validRole);

            return user;
        }
    }
}