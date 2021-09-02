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
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string? PhoneNumber { get; private set; }
        public Guid PhotoId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Role { get; private set; }

        public User(Guid id, string username, string? firstName, string? lastName, string email, string password,
            string? phoneNumber, Guid photoId, DateTime createdAt, string role)
        {
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

            Validate();
        }

        public static User Create(Guid id, string username, string? firstName, string? lastName, string email,
            string password, DateTime createdAt, string role)
        {
            User user = new User(id, username, firstName, lastName, email, password, phoneNumber: null, photoId: Guid.Empty, 
                createdAt, role);

            user.AddEvent(new UserCreated(user));
            return user;
        }

        public void Update(string username, string? firstName, string? lastName, string? phoneNumber, string role)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Role = role;

            Validate();

            AddEvent(new UserUpdated(this));
        }
        
        public void UpdatePhoto(Guid photoId)
        {
            PhotoId = photoId;
            
            AddEvent(new UserUpdated(this));
        }

        public void UpdatePassword(string password)
        {
            Password = password;

            AddEvent(new UserUpdated(this));
        }

        public void UpdateEmail(string email)
        {
            Email = email;
            ValidateEmail();

            AddEvent(new UserUpdated(this));
        }

        public void Delete()
        {
            AddEvent(new UserDeleted(this));
        }

        private void Validate()
        {
            ValidateUsername();
            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();
            ValidatePhoneNumber();
        }

        private void ValidatePhoneNumber()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber)) return;

            if (!PhoneNumberRegex.IsMatch(PhoneNumber))
            {
                throw new InvalidPhoneNumberException(PhoneNumber);
            }
        }

        private void ValidateEmail()
        {
            if (!EmailRegex.IsMatch(Email))
            {
                throw new InvalidEmailValueException(Email);
            }
        }

        private void ValidateFirstName()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return;

            if (FirstName.Length < 2)
            {
                throw new TooShortUserFirstNameException(FirstName);
            }

            if (FirstName.Length > 50)
            {
                throw new TooLongUserFirstNameException(FirstName);
            }
        }

        private void ValidateLastName()
        {
            if (string.IsNullOrWhiteSpace(LastName)) return;

            if (LastName.Length < 2)
            {
                throw new TooShortUserLastNameException(LastName);
            }

            if (LastName.Length > 50)
            {
                throw new TooLongUserLastNameException(LastName);
            }
        }

        private void ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                throw new InvalidUsernameValueException(Username);
            }

            if (Username.Length < 2)
            {
                throw new UsernameTooShortException(Username);
            }
        }

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex PhoneNumberRegex =
            new Regex(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}