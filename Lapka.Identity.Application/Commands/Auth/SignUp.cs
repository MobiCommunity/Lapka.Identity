#nullable enable
using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

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
        public DateTime CreatedAt { get; }

        public SignUp(Guid id, string username, string firstName, string lastName, string email, string password, DateTime createdAt)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            CreatedAt = createdAt;
        }
    }
}