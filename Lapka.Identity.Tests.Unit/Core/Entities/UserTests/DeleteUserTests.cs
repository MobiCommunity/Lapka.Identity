using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class DeleteUserTests
    {
        [Fact]
        public void given_valid_user_should_be_deleted()
        {
            User user = Extensions.ArrangeUser();
            
            user.ShouldNotBeNull();
            user.Delete();
            
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserDeleted>();
        }
    }
}