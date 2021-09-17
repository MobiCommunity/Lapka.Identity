using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.Dashboards
{
    public class IncrementShelterViews : ICommand
    {
        public Guid ShelterId { get; }

        public IncrementShelterViews(Guid shelterId)
        {
            ShelterId = shelterId;
        }
    }
}