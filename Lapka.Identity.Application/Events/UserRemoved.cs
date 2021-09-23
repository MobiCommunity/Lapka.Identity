using System;
using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class UserRemoved : IEvent
    {
        public Guid UserId { get; }

        public UserRemoved(Guid userId)
        {
            UserId = userId;
        }
    }
}