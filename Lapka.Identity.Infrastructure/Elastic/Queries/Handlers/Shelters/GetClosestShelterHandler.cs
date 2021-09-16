using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Shelters
{
    public class GetClosestShelterHandler : IQueryHandler<GetClosestShelter, ShelterDto>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetClosestShelterHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<ShelterDto> HandleAsync(GetClosestShelter query)
        {
            List<ShelterDocument> shelters = await GetSheltersAsync();

            return shelters.Select(x => x.AsDto(query.Latitude, query.Longitude)).OrderBy(x => x.Distance)
                .FirstOrDefault();
        }

        private async Task<List<ShelterDocument>> GetSheltersAsync()
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.Shelters);

            ISearchResponse<ShelterDocument>
                shelters = await _elasticClient.SearchAsync<ShelterDocument>(searchRequest);

            return shelters?.Documents.ToList();
        }
    }
}