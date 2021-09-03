using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class GiveShelterRole : ICommand
    {
        public Guid UserId { get; }

        public GiveShelterRole(Guid userId)
        {
            UserId = userId;
        }
    }
}