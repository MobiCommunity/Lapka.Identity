using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Shelters
{
    public class GetShelterElasticHandler : IQueryHandler<GetShelterElastic, ShelterDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterElasticHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task<ShelterDto> HandleAsync(GetShelterElastic query)
        {
            ShelterDocument shelter = await GetShelterAsync(query);
            
            return shelter.AsDto(query.Latitude, query.Longitude);
        }

        private async Task<ShelterDocument> GetShelterAsync(GetShelterElastic query)
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