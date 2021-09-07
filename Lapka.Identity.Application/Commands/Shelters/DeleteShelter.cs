using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models;

namespace Lapka.Identity.Application.Commands.Shelters
{
    public class DeleteShelter : ICommand
    {
        public Guid Id { get; }
        public UserAuth UserAuth { get; }

        public DeleteShelter(Guid id, UserAuth userAuth)
        {
            Id = id;
            UserAuth = userAuth;
        }
    }
}