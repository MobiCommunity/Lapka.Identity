using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Users
{
    public class UpdateUserPhoto : ICommand
    {
        public Guid UserId { get; }
        public File Photo { get; }

        public UpdateUserPhoto(Guid userId, File photo)
        {
            UserId = userId;
            Photo = photo;
        }
    }
}