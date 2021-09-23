using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Users
{
    public class UserPhotoUpdated : IDomainEvent
    {
        public User User { get; }
        public string PhotoPath { get; }

        public UserPhotoUpdated(User user, string photoPath)
        {
            User = user;
            PhotoPath = photoPath;
        }
    }
}