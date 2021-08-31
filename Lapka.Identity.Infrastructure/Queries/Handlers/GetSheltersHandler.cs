using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.Entities;
using Microsoft.AspNetCore.Routing.Matching;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
{
    public class GetSheltersHandler : IQueryHandler<GetShelters, IEnumerable<ShelterDto>>
    {
        private readonly IShelterRepository _repository;

        public GetSheltersHandler(IShelterRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ShelterDto>> HandleAsync(GetShelters query)
        {
            IEnumerable<Shelter> shelters = await _repository.GetAllAsync();
            return shelters.Select(s => s.AsDto());
        }
    }
}