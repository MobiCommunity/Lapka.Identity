using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class UpdateUser
    {
        [Fact]
        public void given_valid_user_should_be_updated()
        {
            User user = ArrangeUser();
            User updatedUser = ArrangeUser(username: "Develep", firstName: "Mikolaj", lastName: "Samurajski",
                password: "Mynewpassowrd", role: "admin");
            string photoPath = Guid.NewGuid().ToString();

            user.Update(updatedUser.Username, updatedUser.FirstName, updatedUser.LastName, updatedUser.PhoneNumber,
                updatedUser.Role, photoPath);

            user.ShouldNotBeNull();
            user.Username.ShouldBe(updatedUser.Username);
            user.FirstName.ShouldBe(updatedUser.FirstName);
            user.LastName.ShouldBe(updatedUser.LastName);
            user.Role.ShouldBe(updatedUser.Role);
            user.PhoneNumber.ShouldBe(updatedUser.PhoneNumber);
            user.PhotoPath.ShouldBe(photoPath);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }

        public void given_valid_user_email_should_be_updated()
        {
            User user = ArrangeUser();
            User updatedUser = ArrangeUser(username: "Develep", firstName: "Mikolaj", email: "mynewemail@email.com",
                lastName: "Samurajski", password: "Mynewpassowrd", role: "admin");

            user.UpdateEmail(updatedUser.Email);

            user.ShouldNotBeNull();
            user.Email.ShouldBe(updatedUser.Email);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }

        private User ArrangeUser(AggregateId id = null, string username = null, string firstName = null,
            string lastName = null, string email = null, string password = null, DateTime createdAt = default,
            string role = null, List<Guid> userPets = null)
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
            List<Guid> ValidUserPets = userPets ?? new List<Guid>();

            User user = new User(validId.Value, validUsername, validFirstName, validLastName, validEmail, validPassword,
                null, null, validCreatedAt, validRole, ValidUserPets);

            return user;
        }
    }
}