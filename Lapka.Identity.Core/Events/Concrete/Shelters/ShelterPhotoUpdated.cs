using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Shelters
{
    public class ShelterPhotoUpdated : IDomainEvent
    {
        public Shelter Shelter { get; }
        public string OldPhotoPath { get; }

        public ShelterPhotoUpdated(Shelter shelter, string oldPhotoPath)
        {
            Shelter = shelter;
            OldPhotoPath = oldPhotoPath;
        }
    }
}