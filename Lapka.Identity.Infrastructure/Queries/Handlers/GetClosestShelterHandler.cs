using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
{
    public class GetClosestShelterHandler : IQueryHandler<GetClosestShelter, ShelterDto>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public GetClosestShelterHandler(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ShelterDto> HandleAsync(GetClosestShelter query)
        {
            IReadOnlyList<ShelterDocument> shelters = await _repository.FindAsync(_ => true);

            return shelters.Select(x => x.AsDto(query.Latitude, query.Longitude)).OrderBy(x => x.Distance)
                .FirstOrDefault();
        }
    }
}