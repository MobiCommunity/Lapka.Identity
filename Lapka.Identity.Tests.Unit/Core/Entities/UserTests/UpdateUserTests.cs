using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.User;
using Lapka.Identity.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class UpdateUserTests
    {
        [Theory]
        [InlineData("Develep", "Mikolaj", "Samurajski", "123123123")]
        [InlineData("De", "Mikolaj", "Samurajski", "523123123")]
        [InlineData("Develep", "Mi", "Samurajski", "123123123")]
        [InlineData("Develep", "Mikolaj", "Si", "123123123")]
        [InlineData("Develep", "Mikolaj", "Samurajski", "")]
        [InlineData("Develep", "", "", "")]
        [InlineData("Develep", null, null, null)]
        public void given_valid_user_should_be_updated(string username, string firstName, string lastName, string phoneNumber)
        {
            User user = Extensions.ArrangeUser();

            user.Update(username, firstName, lastName, phoneNumber);

            user.ShouldNotBeNull();
            user.Username.ShouldBe(username);
            user.FirstName.ShouldBe(firstName);
            user.LastName.ShouldBe(lastName);
            user.PhoneNumber.ShouldBe(phoneNumber);
            user.Events.Count().ShouldBe(1);
            IDomainEvent @event = user.Events.Single();
            @event.ShouldBeOfType<UserUpdated>();
        }
        
        [Fact]
        public void given_invalid_user_username_should_throw_an_exception()
        {
            User user = Extensions.ArrangeUser();
            Exception exception = Record.Exception(() => user.Update("", user.FirstName, user.LastName, user.PhoneNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidUsernameValueException>();
        }
        
        [Fact]
        public void given_too_short_user_firstname_should_throw_an_exception()
        {
            User user = Extensions.ArrangeUser();

            Exception exception = Record.Exception(() => user.Update(user.Username, "I", user.LastName, user.PhoneNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooShortUserFirstNameException>();
        }
        
        [Fact]
        public void given_too_long_user_firstname_should_throw_an_exception()
        {
            string tooLongFirstName = new string('a', 21);
            User user = Extensions.ArrangeUser();

            Exception exception = Record.Exception(() => user.Update(user.Username, tooLongFirstName, user.LastName, user.PhoneNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooLongUserFirstNameException>();
        }
        
        [Fact]
        public void given_too_short_user_lastname_should_throw_an_exception()
        {
            User user = Extensions.ArrangeUser();

            Exception exception = Record.Exception(() => user.Update(user.Username, user.FirstName, "I", user.PhoneNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooShortUserLastNameException>();
        }
        
        [Fact]
        public void given_too_long_user_lastname_should_throw_an_exception()
        {
            string tooLongLastName = new string('a', 21);
            User user = Extensions.ArrangeUser();

            Exception exception = Record.Exception(() => user.Update(user.Username, user.FirstName, tooLongLastName, user.PhoneNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooLongUserLastNameException>();
        }
        
        [Theory]
        [InlineData("12312312")]
        [InlineData("1231231231")]
        [InlineData("1")]
        [InlineData("12312312a")]
        public void given_invalid_user_phone_number_should_throw_an_exception(string phoneNumber)
        {
            User user = Extensions.ArrangeUser();

            Exception exception = Record.Exception(() => user.Update(user.Username, user.FirstName, user.LastName, phoneNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPhoneNumberException>();
        }
    }
}