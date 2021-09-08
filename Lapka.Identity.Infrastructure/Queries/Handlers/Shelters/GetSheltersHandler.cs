using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers.Shelters
{
    public class GetSheltersHandler : IQueryHandler<GetShelters, IEnumerable<ShelterDto>>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public GetSheltersHandler(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ShelterDto>> HandleAsync(GetShelters query)
        {
            IEnumerable<ShelterDocument> shelters = await _repository.FindAsync(_ => true);
            
            return shelters.Select(s => s.AsDto(query.Latitude, query.Longitude));
        }
    }
}