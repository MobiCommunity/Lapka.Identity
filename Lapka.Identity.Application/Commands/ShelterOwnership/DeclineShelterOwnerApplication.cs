using System;
using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.ShelterOwnership
{
    public class DeclineShelterOwnerApplication : ICommand
    {
        public Guid Id { get; }

        public DeclineShelterOwnerApplication(Guid id)
        {
            Id = id;
        }
    }
}