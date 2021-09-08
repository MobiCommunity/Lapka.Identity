using System;
using Convey.CQRS.Queries;

namespace Lapka.Identity.Application.Queries
{
    public class CheckUserShelterOwnership : IQuery<bool>
    {
        public Guid ShelterId { get; set; }
        public Guid UserId { get; set; }
    }
}