using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.ShelterOwnership
{
    public class RemoveUserFromShelterOwners : ICommand
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }

        public RemoveUserFromShelterOwners(Guid userId, Guid shelterId)
        {
            UserId = userId;
            ShelterId = shelterId;
        }
    }
}