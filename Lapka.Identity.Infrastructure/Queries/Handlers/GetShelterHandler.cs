using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
{
    public class GetShelterHandler : IQueryHandler<GetShelter, ShelterDto>
    {
        private readonly IShelterRepository _shelterRepository;

        public GetShelterHandler(IShelterRepository shelterRepository)
        {
            _shelterRepository = shelterRepository;
        }
        
        public async Task<ShelterDto> HandleAsync(GetShelter query)
        {
            Shelter shelter = await _shelterRepository.GetByIdAsync(query.Id);
            return shelter.AsDto();
        }
    }
}