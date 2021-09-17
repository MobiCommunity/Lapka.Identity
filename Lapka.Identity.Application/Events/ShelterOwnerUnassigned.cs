using System;
using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class ShelterOwnerUnassigned : IEvent
    {
        public Guid ShelterId { get; }
        public Guid UserId { get; }

        public ShelterOwnerUnassigned(Guid shelterId, Guid userId)
        {
            ShelterId = shelterId;
            UserId = userId;
        }
    }
}