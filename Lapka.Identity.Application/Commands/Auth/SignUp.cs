using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.Auth
{
    public class SignUp : ICommand
    {
        public SignUp(Guid id, string username, string firstName, string lastName, string email, string password,
            DateTime createdAt, string role)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            CreatedAt = createdAt;
            Role = role;
        }

        public Guid Id { get; }
        public string Username { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Password { get; }
        public DateTime CreatedAt { get; }
        public string Role { get; }
    }
}