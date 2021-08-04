using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete
{
    public class ShelterUpdated : IDomainEvent
    {
        public Shelter Shelter { get; }

        public ShelterUpdated(Shelter shelter)
        {
            Shelter = shelter;
        }
    }
}