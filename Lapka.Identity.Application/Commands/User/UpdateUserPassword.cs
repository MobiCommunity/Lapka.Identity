using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class UpdateUserPassword : ICommand
    {
        public Guid Id { get; }
        public string Password { get; }

        public UpdateUserPassword(Guid id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}