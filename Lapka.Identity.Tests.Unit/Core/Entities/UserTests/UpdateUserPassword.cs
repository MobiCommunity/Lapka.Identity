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
    public class UpdateUserPassword
    {
        [Fact]
        public void given_valid_user_password_should_be_updated()
        {
            const string newPassword = "Newsecretpassword"; 
            User user = Extensions.ArrangeUser();
            
            user.UpdatePassword(newPassword);

            user.ShouldNotBeNull();
            user.Password.ShouldBe(newPassword);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }
    }
}