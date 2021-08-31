using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class DeleteUser : ICommand
    {
        public Guid Id { get; }

        public DeleteUser(Guid id)
        {
            Id = id;
        }
    }
}