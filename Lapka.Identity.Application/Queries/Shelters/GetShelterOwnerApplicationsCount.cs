using System;
using Convey.CQRS.Queries;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetShelterOwnerApplicationsCount : IQuery<long>
    {
        public Guid ShelterId { get; set; }
    }
}