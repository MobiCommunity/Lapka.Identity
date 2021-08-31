using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete
{
    public class ShelterPhotoUpdated : IDomainEvent
    {
        public Shelter Shelter { get; }

        public ShelterPhotoUpdated(Shelter shelter)
        {
            Shelter = shelter;
        }
    }
}