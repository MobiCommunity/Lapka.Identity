using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete
{
    public class ShelterCreated : IDomainEvent
    {
        public Shelter Shelter { get; }

        public ShelterCreated(Shelter shelter)
        {
            Shelter = shelter;  
        }
    }
}