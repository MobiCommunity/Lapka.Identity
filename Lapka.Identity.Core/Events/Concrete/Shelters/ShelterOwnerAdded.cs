using System;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Shelters
{
    public class ShelterOwnerAdded : IDomainEvent
    {
        public Shelter Shelter { get; }
        public Guid OwnerId { get; }

        public ShelterOwnerAdded(Shelter shelter, Guid ownerId)
        {
            Shelter = shelter;
            OwnerId = ownerId;
        }
    }
}