using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Users
{
    public class UserDeleted : IDomainEvent
    {
        public User User { get; }

        public UserDeleted(User user)
        {
            User = user;
        }
    }
}