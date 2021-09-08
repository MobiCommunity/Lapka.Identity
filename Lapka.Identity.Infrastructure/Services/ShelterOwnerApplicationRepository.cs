using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Documents;
using Lapka.Identity.Infrastructure.Exceptions;

namespace Lapka.Identity.Infrastructure.Services
{
    public class ShelterOwnerApplicationRepository : IShelterOwnerApplicationRepository
    {
        private readonly IMongoRepository<ShelterOwnerApplicationDocument, Guid> _repository;

        public ShelterOwnerApplicationRepository(IMongoRepository<ShelterOwnerApplicationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ShelterOwnerApplication> GetAsync(Guid id)
        {
            ShelterOwnerApplicationDocument application = await _repository.GetAsync(id);
            
            if (application is null)
            {
                return null;
            }
            return application.AsBusiness();
        }

        public async Task<ShelterOwnerApplication> GetAsync(Guid userId, Guid shelterId)
        {
            ShelterOwnerApplicationDocument application =
                await _repository.GetAsync(x => x.UserId == userId && x.ShelterId == shelterId);
            
            if (application is null)
            {
                return null;
            }
            
            return application.AsBusiness();
        }

        public async Task AddApplicationAsync(ShelterOwnerApplication application)
        {
            await _repository.AddAsync(application.AsDocument());
        }

        public async Task UpdateAsync(ShelterOwnerApplication application)
        {
            await _repository.UpdateAsync(application.AsDocument());
        }
    }
}