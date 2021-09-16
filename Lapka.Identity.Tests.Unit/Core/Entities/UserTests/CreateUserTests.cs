using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Users;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.User;
using Lapka.Identity.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.UserTests
{
    public class CreateUserTests
    {
        private User Act(AggregateId id, string username, string firstName, string lastName, string email,
            string password, DateTime createdAt, string role) => User.Create(id.Value, username, firstName, lastName,
            email, password, createdAt, role);

        [Theory]
        [InlineData("Ma", "Nick", "Yellow", "my@email.com", "secretpassword")]
        [InlineData("Ma", "Nick", "Yellow", "my@email.com", "secret")]
        [InlineData("Ma", "Na", "Ye", "my@email.com", "secretpassword")]
        public void given_valid_user_should_be_created(string username, string firstName, string lastName, string email,
            string password)
        {
            User arrangeUser = Extensions.ArrangeUser(username: username, firstName: firstName, lastName: lastName,
                email: email, password: password);

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

        [Theory]
        [InlineData("t.com")]
        [InlineData("t@.com")]
        [InlineData("t@com")]
        [InlineData("")]
        [InlineData(" ")]
        public void given_invalid_user_email_should_throw_an_exception(string invalidEmail)
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(email: invalidEmail));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidEmailValueException>();
        }

        [Fact]
        public void given_too_short_user_username_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(username: "a"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UsernameTooShortException>();
        }
        
        [Fact]
        public void given_too_long_user_username_should_throw_an_exception()
        {
            string tooLongUsername = new string('a', 21);

            Exception exception = Record.Exception(() => Extensions.ArrangeUser(username: tooLongUsername));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<UsernameTooLongException>();
        }
        
        [Fact]
        public void given_too_short_user_password_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(password: "12345"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooShortPasswordException>();
        }

        [Fact]
        public void given_invalid_user_username_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(username: ""));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidUsernameValueException>();
        }
        
        [Fact]
        public void given_too_short_user_firstname_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(firstName: "I"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooShortUserFirstNameException>();
        }
        
        [Fact]
        public void given_too_long_user_firstname_should_throw_an_exception()
        {
            string tooLongFirstName = new string('a', 21);
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(firstName: tooLongFirstName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooLongUserFirstNameException>();
        }
        
        [Fact]
        public void given_too_short_user_lastname_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(lastName: "I"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooShortUserLastNameException>();
        }
        
        [Fact]
        public void given_too_long_user_lastname_should_throw_an_exception()
        {
            string tooLongLastName = new string('a', 21);
            Exception exception = Record.Exception(() => Extensions.ArrangeUser(lastName: tooLongLastName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<TooLongUserLastNameException>();
        }
    }
}