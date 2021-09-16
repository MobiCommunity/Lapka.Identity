using System;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetShelter : IQuery<ShelterDto>
    {
        public Guid Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}