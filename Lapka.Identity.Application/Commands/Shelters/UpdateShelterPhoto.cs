using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Shelters
{
    public class UpdateShelterPhoto : ICommand
    {
        public Guid Id { get; }
        public UserAuth UserAuth { get; }
        public File Photo { get; }

        public UpdateShelterPhoto(Guid id, UserAuth userAuth, File photo)
        {
            Id = id;
            UserAuth = userAuth;
            Photo = photo;
        }
    }
}