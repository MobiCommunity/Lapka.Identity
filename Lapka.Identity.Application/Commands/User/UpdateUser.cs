using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class UpdateUser : ICommand
    {
        public Guid Id { get; }
        public string Username { get; }
        public string FirstName { get;}
        public string LastName{ get; }
        public string? PhoneNumber { get; }

        public UpdateUser(Guid id, string username, string firstName, string lastName, string? phoneNumber)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }
}