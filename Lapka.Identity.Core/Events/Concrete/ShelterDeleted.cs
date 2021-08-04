using System;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete
{
    public class ShelterDeleted : IDomainEvent
    {
        public Shelter Shelter { get; }
        public ShelterDeleted(Shelter shelter)
        {
            Shelter = shelter;
        }
    }
}