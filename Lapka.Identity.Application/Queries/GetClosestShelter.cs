using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries
{
    public class GetClosestShelter : IQuery<ShelterDto>
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}