using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands
{
    public class UpdateUserPhoto : ICommand
    {
        public Guid UserId { get; }
        public PhotoFile Photo { get; }

        public UpdateUserPhoto(Guid userId, PhotoFile photo)
        {
            UserId = userId;
            Photo = photo;
        }
    }
}