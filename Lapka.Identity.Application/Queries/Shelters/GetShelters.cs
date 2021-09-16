using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetShelters : IQuery<IEnumerable<ShelterDto>>
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}