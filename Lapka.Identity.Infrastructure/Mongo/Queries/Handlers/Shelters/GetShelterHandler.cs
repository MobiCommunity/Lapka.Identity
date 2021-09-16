using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries.Handlers.Shelters
{
    public class GetShelterHandler : IQueryHandler<GetShelter, ShelterDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task<ShelterDto> HandleAsync(GetShelter query)
        {
            ShelterDocument shelter = await GetShelterAsync(query);
            
            return shelter.AsDto(query.Latitude, query.Longitude);
        }

        private async Task<ShelterDocument> GetShelterAsync(GetShelter query)
        {
            GetResponse<ShelterDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<ShelterDocument>(new Id(query.Id.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.Shelters));

            ShelterDocument shelter = response?.Source;
            if (shelter is null)
            {
                throw new ShelterNotFoundException(query.Id.ToString());
            }
            return shelter;
        }
    }
}