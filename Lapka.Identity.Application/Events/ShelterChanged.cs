using System;
using Convey.CQRS.Events;

namespace Lapka.Identity.Application.Events
{
    public class ShelterChanged : IEvent
    {
        public Guid ShelterId { get; }

        public ShelterChanged(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}