using System;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Shelters
{
    public class ShelterOwnerRemoved : IDomainEvent
    {
        public Shelter Shelter { get; }
        public Guid OwnerId { get; }

        public ShelterOwnerRemoved(Shelter shelter, Guid ownerId)
        {
            Shelter = shelter;
            OwnerId = ownerId;
        }
    }
}