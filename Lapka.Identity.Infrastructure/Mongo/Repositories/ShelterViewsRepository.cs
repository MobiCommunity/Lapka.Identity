using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Mongo.Documents;

namespace Lapka.Identity.Infrastructure.Mongo.Repositories
{
    public class ShelterViewsRepository : IShelterViewsRepository
    {
        private readonly IMongoRepository<ShelterViewsDocument, Guid> _repository;
        
        public ShelterViewsRepository(IMongoRepository<ShelterViewsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(ShelterViews view)
        {
            await _repository.AddAsync(view.AsDocument());
        }

        public async Task DeleteAsync(ShelterViews view)
        {
            await _repository.DeleteAsync(view.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateAsync(ShelterViews view)
        {
            await _repository.UpdateAsync(view.AsDocument());
        }

        public async Task<ShelterViews> GetByIdAsync(Guid id)
        {
            ShelterViewsDocument viewsFromDb = await _repository.GetAsync(id);
            if (viewsFromDb is null)
            {
                return null;
            }

            return viewsFromDb.AsBusiness();
        } 
    }
}