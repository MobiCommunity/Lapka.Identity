using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Users
{
    public class UserUpdated : IDomainEvent
    {
        public User User { get; }

        public UserUpdated(User user)
        {
            User = user;
        }
    }
}