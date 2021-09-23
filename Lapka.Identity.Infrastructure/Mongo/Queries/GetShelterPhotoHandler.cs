using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries
{
    public class GetShelterPhotoHandler : IQueryHandler<GetShelterPhoto, string>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public GetShelterPhotoHandler(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<string> HandleAsync(GetShelterPhoto query)
        {
            ShelterDocument shelter = await GetShelterAsync(query);
            
            return shelter.PhotoPath;
        }

        private async Task<ShelterDocument> GetShelterAsync(GetShelterPhoto query)
        {
            ShelterDocument shelter = await _repository.GetAsync(query.Id);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(query.Id.ToString());
            }
            
            return shelter;
        }
    }
}