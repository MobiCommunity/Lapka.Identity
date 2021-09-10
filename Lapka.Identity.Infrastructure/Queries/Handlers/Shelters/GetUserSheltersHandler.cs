using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Identity.Infrastructure.Queries.Handlers.Shelters
{
    public class GetUserSheltersHandler : IQueryHandler<GetUserShelters, IEnumerable<ShelterBasicDto>>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public GetUserSheltersHandler(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ShelterBasicDto>> HandleAsync(GetUserShelters query)
        {
            IReadOnlyList<ShelterDocument> userShelters =
                await _repository.FindAsync(x => x.Owners.Any(y => y == query.UserId));

            return userShelters.Select(x => x.AsDto());
        }
    }
}