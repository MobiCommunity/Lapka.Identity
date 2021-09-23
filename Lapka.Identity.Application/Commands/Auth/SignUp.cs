using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Auth
{
    public class SignUp : ICommand
    {
        public Guid Id { get; }
        public string Username { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public EmailAddress Email { get; }
        public string Password { get; }
        public DateTime CreatedAt { get; }
        public string Role { get; }
        public string PhotoPath { get; }
        
        public SignUp(Guid id, string username, string firstName, string lastName, EmailAddress email, string password,
            DateTime createdAt, string role, string photoPath = null)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            CreatedAt = createdAt;
            Role = role;
            PhotoPath = photoPath;
        }


    }
}