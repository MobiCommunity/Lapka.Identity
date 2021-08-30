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
            User user = Extensions.ArrangeUser();
            User updatedUser = Extensions.ArrangeUser(username: "Develep", firstName: "Mikolaj", lastName: "Samurajski",
                password: "Mynewpassowrd", role: "admin");

            user.Update(updatedUser.Username, updatedUser.FirstName, updatedUser.LastName, updatedUser.PhoneNumber,
                updatedUser.Role);

            user.ShouldNotBeNull();
            user.Username.ShouldBe(updatedUser.Username);
            user.FirstName.ShouldBe(updatedUser.FirstName);
            user.LastName.ShouldBe(updatedUser.LastName);
            user.Role.ShouldBe(updatedUser.Role);
            user.PhoneNumber.ShouldBe(updatedUser.PhoneNumber);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }

        public void given_valid_user_email_should_be_updated()
        {
            User user = Extensions.ArrangeUser();
            User updatedUser = Extensions.ArrangeUser(username: "Develep", firstName: "Mikolaj", email: "mynewemail@email.com",
                lastName: "Samurajski", password: "Mynewpassowrd", role: "admin");

            user.UpdateEmail(updatedUser.Email);

            user.ShouldNotBeNull();
            user.Email.ShouldBe(updatedUser.Email);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }
    }
}