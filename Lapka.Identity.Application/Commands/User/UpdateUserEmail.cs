using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
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