using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Core.Entities;
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
            User arrangeUser = ArrangeUser();
            
            SignUp command = new SignUp(id, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, createdAt);

            await Act(command);

            await _identityService.Received().SignUpAsync(Arg.Is(command));
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