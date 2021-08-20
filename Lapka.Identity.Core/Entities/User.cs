#nullable enable
using System;
using System.Collections.Generic;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Username { get; }
        public string FirstName { get; }
        public string LastName{ get; }
        public string Email { get; }
        public string Password { get; }
        public string? PhoneNumber { get; }
        public string? PhotoPath { get; }
        public DateTime CreatedAt { get; }
        public string Role { get; }

        public User(Guid id, string username, string firstName, string lastName, string email, string password, 
            string phoneNumber, string photoPath, DateTime createdAt, string role)
        {
            Id = new AggregateId(id);
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

        public static User Create(Guid id, string username, string firstName, string lastName, string email, 
            string password, string? phoneNumber, string? photoPath, DateTime createdAt, string role)
        {
            User user = new User(id, username, firstName, lastName, email, password, phoneNumber, photoPath, createdAt, role);
            user.AddEvent(new UserCreated(user));
            return user;
        }
    }
}