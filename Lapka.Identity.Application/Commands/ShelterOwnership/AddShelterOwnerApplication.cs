using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.ShelterOwnership
{
    public class AddShelterOwnerApplication : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid ShelterId { get; }

        public AddShelterOwnerApplication(Guid id, Guid userId, Guid shelterId)
        {
            Id = id;
            UserId = userId;
            ShelterId = shelterId;
        }
    }
}