using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Users;
using Lapka.Identity.Core.Exceptions;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class UpdateUserEmailTests
    {
        [Fact]
        public void given_valid_user_email_should_be_updated()
        {
            User user = Extensions.ArrangeUser();
            string mailToUpdate = "mynewemail@email.com";

            user.UpdateEmail(mailToUpdate);

            user.ShouldNotBeNull();
            user.Email.ShouldBe(mailToUpdate);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }

        [Theory]
        [InlineData("t.com")]
        [InlineData("t@.com")]
        [InlineData("t@com")]
        [InlineData("")]
        [InlineData(" ")]
        public void given_invalid_user_email_should_throw_an_exception(string email)
        {
            User user = Extensions.ArrangeUser();

            Exception exception = Record.Exception(() => user.UpdateEmail(email));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidEmailValueException>();
        }
    }
}