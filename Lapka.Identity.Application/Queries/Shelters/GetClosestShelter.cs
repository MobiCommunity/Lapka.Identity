using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto.Shelters;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetClosestShelter : IQuery<ShelterDto>
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}