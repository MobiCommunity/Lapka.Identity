using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands
{
    public class UpdateUserPhoto : ICommand
    {
        public Guid UserId { get; }
        public File Photo { get; }
        public Guid PhotoId { get; }

        public UpdateUserPhoto(Guid userId, File photo, Guid photoId)
        {
            UserId = userId;
            Photo = photo;
            PhotoId = photoId;
        }
    }
}