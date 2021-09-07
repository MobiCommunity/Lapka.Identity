using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
{
    public class GetShelterOwnerApplicationsHandler : IQueryHandler<GetShelterOwnerApplications,
        IEnumerable<ShelterOwnerApplicationDto>>
    {
        private readonly IMongoRepository<ShelterOwnerApplicationDocument, Guid> _repository;

        public GetShelterOwnerApplicationsHandler(IMongoRepository<ShelterOwnerApplicationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ShelterOwnerApplicationDto>> HandleAsync(GetShelterOwnerApplications query)
        {
            IReadOnlyList<ShelterOwnerApplicationDocument> applications = await _repository.FindAsync(_ => true);

            return applications.Select(x => x.AsDto());
        }
    }
}