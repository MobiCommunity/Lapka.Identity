using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class CreateUser
    {
        private User Act(AggregateId id, string username, string firstName, string lastName, string email,
            string password, DateTime createdAt, string role) => User.Create(id.Value, username, firstName, lastName,
            email, password, createdAt, role);

        [Fact]
        public void given_valid_user_should_be_created()
        {
            User arrangeUser = ArrangeUser();

            User user = Act(arrangeUser.Id, arrangeUser.Username, arrangeUser.FirstName, arrangeUser.LastName,
                arrangeUser.Email, arrangeUser.Password, arrangeUser.CreatedAt, arrangeUser.Role);

            user.ShouldNotBeNull();
            user.Id.ShouldBe(user.Id);
            user.Username.ShouldBe(user.Username);
            user.FirstName.ShouldBe(user.FirstName);
            user.LastName.ShouldBe(user.LastName);
            user.Email.ShouldBe(user.Email);
            user.Password.ShouldBe(user.Password);
            user.CreatedAt.ShouldBe(user.CreatedAt);
            user.Role.ShouldBe(user.Role);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserCreated>();
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