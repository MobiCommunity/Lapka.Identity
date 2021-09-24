using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetSpecificShelterOwnerApplications : IQuery<IEnumerable<ShelterOwnerApplicationDto>>
    {
        public Guid ShelterId { get; set; }
    }
}