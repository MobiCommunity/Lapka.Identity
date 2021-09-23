using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class UserPhotoRemoved : IEvent
    {
        public string PhotoPath { get; }

        public UserPhotoRemoved(string photoPath)
        {
            PhotoPath = photoPath;
        }
    }
}