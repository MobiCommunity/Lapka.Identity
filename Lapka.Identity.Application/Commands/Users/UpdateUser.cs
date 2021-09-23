using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Users
{
    public class UpdateUser : ICommand
    {
        public Guid Id { get; }
        public string Username { get; }
        public string FirstName { get;}
        public string LastName{ get; }
        public PhoneNumber PhoneNumber { get; }

        public UpdateUser(Guid id, string username, string firstName, string lastName, PhoneNumber phoneNumber = null)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }
}