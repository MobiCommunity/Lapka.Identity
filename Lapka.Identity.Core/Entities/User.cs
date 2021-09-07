#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lapka.Identity.Core.Events.Concrete;
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
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string PhoneNumber { get; private set; }
        public Guid PhotoId { get; private set; }
        public DateTime CreatedAt { get; }
        public string Role { get; private set; }

        public User(Guid id, string username, string firstName, string lastName, string email, string password,
            string phoneNumber, Guid photoId, DateTime createdAt, string role)
        {
            Validate(username, firstName, lastName, phoneNumber);
            ValidateEmail(email);
            ValidatePassword(password);
            
            Id = new AggregateId(id);
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            PhotoId = photoId;
            CreatedAt = createdAt;
            Role = role;
        }

        public static User Create(Guid id, string username, string firstName, string lastName, string email,
            string password, DateTime createdAt, string role)
        {
            User user = new User(id, username, firstName, lastName, email, password, phoneNumber: string.Empty, photoId: Guid.Empty, 
                createdAt, role);

            user.AddEvent(new UserCreated(user));
            return user;
        }

        public void Update(string username, string firstName, string lastName, string phoneNumber)
        {
            Validate(username, firstName, lastName, phoneNumber);

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
        
        public void UpdatePhoto(Guid photoId)
        {
            PhotoId = photoId;
            
            AddEvent(new UserUpdated(this));
        }

        public void UpdatePassword(string password)
        {
            ValidatePassword(password);
            Password = password;

            AddEvent(new UserUpdated(this));
        }

        public void UpdateEmail(string email)
        {
            ValidateEmail(email);
            Email = email;

            AddEvent(new UserUpdated(this));
        }

        public void Delete()
        {
            AddEvent(new UserDeleted(this));
        }

        private static void Validate(string username, string firstName, string lastName, string phoneNumber)
        {
            ValidateUsername(username);
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidatePhoneNumber(phoneNumber);
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return;

            if (!PhoneNumberRegex.IsMatch(phoneNumber))
            {
                throw new InvalidPhoneNumberException(phoneNumber);
            }
        }

        private static void ValidateEmail(string email)
        {
            if (!EmailRegex.IsMatch(email))
            {
                throw new InvalidEmailValueException(email);
            }
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

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex PhoneNumberRegex =
            new Regex(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}