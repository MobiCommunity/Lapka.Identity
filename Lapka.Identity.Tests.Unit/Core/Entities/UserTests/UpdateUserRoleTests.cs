using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class UpdateUserRoleTests
    {
        [Fact]
        public void given_valid_user_role_should_be_updated()
        {
            User user = Extensions.ArrangeUser();
            string newRole = "shelter";

            user.ChangeRole(newRole);

            user.ShouldNotBeNull();
            user.Role.ShouldBe(newRole);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }

    }
}