using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Services
{
    public class ShelterRepository : IShelterRepository
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;
        public ShelterRepository(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Shelter shelter)
        {
            await _repository.AddAsync(shelter.AsDocument());
        }

        public async Task<IEnumerable<Shelter>> GetAllAsync()
        {
            var sheltersFromDb = await _repository.FindAsync(_ => true);

            return sheltersFromDb.Select(x => x.AsBusiness());
        }
        
        public async Task DeleteAsync(Shelter shelter)
        {
            await _repository.DeleteAsync(shelter.Id.Value);
        }

        public async Task UpdateAsync(Shelter shelter)
        {
            await _repository.UpdateAsync(shelter.AsDocument());
        }

        public async Task<Shelter> GetByIdAsync(Guid id)
        {
            ShelterDocument shelterFromDb = await _repository.GetAsync(id);
            if (shelterFromDb == null) return null;

            return shelterFromDb.AsBusiness();
        } 
    }
}