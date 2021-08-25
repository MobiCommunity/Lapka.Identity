using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands
{
    public class UpdateShelterPhoto : ICommand
    {
        public Guid Id { get; }
        public File Photo { get; }
        public Guid PhotoId { get; }

        public UpdateShelterPhoto(Guid id, File photo, Guid photoId)
        {
            Id = id;
            Photo = photo;
            PhotoId = photoId;
        }
    }
}