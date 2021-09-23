#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Users;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.User;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public EmailAddress Email { get; private set; }
        public string Password { get; private set; }
        public PhoneNumber? PhoneNumber { get; private set; }
        public string? PhotoPath { get; private set; }
        public DateTime CreatedAt { get; }
        public string Role { get; private set; }

        public User(Guid id, string username, string firstName, string lastName, EmailAddress email, string password,
            DateTime createdAt, string role, string? photoPath = null)
        {
            Validate(username, firstName, lastName);
            ValidatePassword(password);

            Id = new AggregateId(id);
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            PhotoPath = photoPath;
            CreatedAt = createdAt;
            Role = role;
        }

        public User(Guid id, string username, string firstName, string lastName, EmailAddress email, string password,
            DateTime createdAt, string role, PhoneNumber phoneNumber, string? photoPath = null) : this(id, username,
            firstName, lastName, email, password, createdAt, role, photoPath)
        {
            PhoneNumber = phoneNumber;
        }

        public static User Create(Guid id, string username, string firstName, string lastName, EmailAddress email,
            string password, DateTime createdAt, string role, PhoneNumber phoneNumber = null!, string? photoPath = null)
        {
            User user = new User(id, username, firstName, lastName, email, password, createdAt, role, phoneNumber,
                photoPath);

            user.AddEvent(new UserCreated(user));
            return user;
        }

        public void Update(string username, string firstName, string lastName, PhoneNumber? phoneNumber)
        {
            Validate(username, firstName, lastName);

            Username = username;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;

            AddEvent(new UserUpdated(this));
        }

        public void ChangeRole(string role)
        {
            Role = role;

            AddEvent(new UserUpdated(this));
        }

        public void UpdatePhoto(string photoPath, string oldPhotoPath)
        {
            PhotoPath = photoPath;

            AddEvent(new UserPhotoUpdated(this, oldPhotoPath));
        }

        public void UpdatePassword(string password)
        {
            ValidatePassword(password);
            Password = password;
        }

        public void UpdateEmail(EmailAddress email)
        {
            Email = email;

            AddEvent(new UserUpdated(this));
        }

        public void Delete()
        {
            AddEvent(new UserDeleted(this));
        }

        private static void Validate(string username, string firstName, string lastName)
        {
            ValidateUsername(username);
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
        }

        private static void ValidatePassword(string password)
        {
            if (password.Length < MinimumPasswordLength)
            {
                throw new TooShortPasswordException();
            }
        }

        private static void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) return;

            if (firstName.Length < MinimumFirstNameLength)
            {
                throw new TooShortUserFirstNameException(firstName);
            }

            if (firstName.Length > MaximumFirstNameLength)
            {
                throw new TooLongUserFirstNameException(firstName);
            }
        }

        private static void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName)) return;

            if (lastName.Length < MinimumLastNameLength)
            {
                throw new TooShortUserLastNameException(lastName);
            }

            if (lastName.Length > MaximumLastNameLength)
            {
                throw new TooLongUserLastNameException(lastName);
            }
        }

        private static void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidUsernameValueException(username);
            }

            if (username.Length < MinimumUsernameLength)
            {
                throw new UsernameTooShortException(username);
            }

            if (username.Length > MaximumUsernameLength)
            {
                throw new UsernameTooLongException(username);
            }
        }

        private const int MinimumPasswordLength = 6;
        private const int MinimumUsernameLength = 2;
        private const int MinimumFirstNameLength = 2;
        private const int MinimumLastNameLength = 2;
        private const int MaximumUsernameLength = 20;
        private const int MaximumFirstNameLength = 20;
        private const int MaximumLastNameLength = 20;
    }
}