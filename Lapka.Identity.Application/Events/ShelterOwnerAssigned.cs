using System;
using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class ShelterOwnerAssigned : IEvent
    {
        public Guid ShelterId { get; }
        public Guid UserId { get; }

        public ShelterOwnerAssigned(Guid shelterId, Guid userId)
        {
            ShelterId = shelterId;
            UserId = userId;
        }
    }
}