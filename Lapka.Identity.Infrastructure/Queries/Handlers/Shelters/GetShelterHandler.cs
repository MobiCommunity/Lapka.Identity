using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers.Shelters
{
    public class GetShelterHandler : IQueryHandler<GetShelter, ShelterDto>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;

        public GetShelterHandler(IMongoRepository<ShelterDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<ShelterDto> HandleAsync(GetShelter query)
        {
            ShelterDocument shelter = await _repository.GetAsync(query.Id);
            if (shelter is null) throw new ShelterNotFoundException(query.Id.ToString());
            
            return shelter.AsDto(query.Latitude, query.Longitude);
        }
    }
}