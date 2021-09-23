using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries
{
    public class GetShelterMongoHandler : IQueryHandler<GetShelterMongo, ShelterDto>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public GetShelterMongoHandler(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<ShelterDto> HandleAsync(GetShelterMongo query)
        {
            ShelterDocument shelter = await GetShelterAsync(query);
            
            return shelter.AsDto(query.Latitude, query.Longitude);
        }

        private async Task<ShelterDocument> GetShelterAsync(GetShelterMongo query)
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