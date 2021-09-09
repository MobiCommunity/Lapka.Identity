using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers.Shelters
{
    public class CheckUserShelterOwnershipHandler : IQueryHandler<CheckUserShelterOwnership, bool>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public CheckUserShelterOwnershipHandler(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<bool> HandleAsync(CheckUserShelterOwnership query)
        {
            ShelterDocument shelter = await _repository.GetAsync(x => x.Id == query.ShelterId);
            if (shelter == null)
            {
                return false;
            }
            
            return shelter.Owners.Any(x => x == query.UserId);
        }
    }
}