using System;
using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class ShelterAdded : IEvent
    {
        public Guid ShelterId { get; }

        public ShelterAdded(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}