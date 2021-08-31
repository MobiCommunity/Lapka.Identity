using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Core.Events.Concrete
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