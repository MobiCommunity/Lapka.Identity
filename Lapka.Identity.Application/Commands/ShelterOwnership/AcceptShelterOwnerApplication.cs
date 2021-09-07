using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.ShelterOwnership
{
    public class AcceptShelterOwnerApplication : ICommand
    {
        public Guid Id { get; }

        public AcceptShelterOwnerApplication(Guid id)
        {
            Id = id;
        }
    }
}