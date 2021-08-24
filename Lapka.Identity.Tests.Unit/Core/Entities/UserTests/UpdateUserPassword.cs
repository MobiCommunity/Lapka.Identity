using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class UpdateUserPassword
    {
        [Fact]
        public void given_valid_user_password_should_be_updated()
        {
            const string newPassword = "Newsecretpassword"; 
            User user = ArrangeUser();
            
            user.UpdatePassword(newPassword);

            user.ShouldNotBeNull();
            user.Password.ShouldBe(newPassword);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
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