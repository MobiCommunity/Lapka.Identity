using System;
using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class ShelterRemoved : IEvent
    {
        public Guid ShelterId { get; }

        public ShelterRemoved(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}