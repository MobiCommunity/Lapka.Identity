using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class SignUp : ICommand
    {
        public Guid Id { get; }
        public string Username { get; }
        public string FirstName { get; }
        public string LastName{ get; }
        public string Email { get; }
        public string Password { get; }
        public string PhoneNumber { get; }
        public string PhotoPath { get; }
        public DateTime CreatedAt { get; }
        public string Role { get; }

        public SignUp(Guid id, string username, string firstName, string lastName, string email, string password,
            string phoneNumber, string photoPath, DateTime createdAt, string role)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            PhotoPath = photoPath;
            CreatedAt = createdAt;
            Role = role;
        }
    }
}