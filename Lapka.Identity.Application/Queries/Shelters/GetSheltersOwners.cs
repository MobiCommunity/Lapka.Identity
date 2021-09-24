using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetSheltersOwners : IQuery<IEnumerable<Guid>>
    {
        public Guid ShelterId { get; set; }
    }
}