using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries
{
    public class GetUserShelters : IQuery<IEnumerable<ShelterBasicDto>>
    {
        public Guid UserId { get; set; }
    }
}