using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Users
{
    public class UpdateUserEmail : ICommand
    {
        public Guid Id { get; }
        public string Email { get; }

        public UpdateUserEmail(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}