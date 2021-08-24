using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class DeleteShelter : ICommand
    {
        public Guid Id { get; }

        public DeleteShelter(Guid id)
        {
            Id = id;
        }
    }
}