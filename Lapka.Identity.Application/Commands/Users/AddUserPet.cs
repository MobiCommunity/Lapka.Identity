using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.Users
{
    public class AddUserPet : ICommand
    {
        public Guid UserId { get; }
        public Guid PetId { get; }

        public AddUserPet(Guid userId, Guid petId)
        {
            UserId = userId;
            PetId = petId;
        }
    }
}