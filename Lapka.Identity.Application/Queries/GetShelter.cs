using System;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries
{
    public class GetShelter : IQuery<ShelterDto>
    {
        public Guid Id { get; set; }
    }
}