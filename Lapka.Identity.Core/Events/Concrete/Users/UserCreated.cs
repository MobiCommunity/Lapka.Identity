using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete.Users
{
    public class UserCreated : IDomainEvent
    {
        public User User { get; }

        public UserCreated(User user)
        {
            User = user;
        }
    }
}