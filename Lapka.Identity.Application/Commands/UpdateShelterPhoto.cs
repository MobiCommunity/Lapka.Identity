using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands
{
    public class UpdateShelterPhoto : ICommand
    {
        public Guid Id { get; }
        public PhotoFile Photo { get; }

        public UpdateShelterPhoto(Guid id, PhotoFile photo)
        {
            Id = id;
            Photo = photo;
        }
    }
}