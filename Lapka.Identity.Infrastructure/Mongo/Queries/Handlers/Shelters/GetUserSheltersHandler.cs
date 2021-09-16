using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries.Handlers.Shelters
{
    public class GetUserSheltersHandler : IQueryHandler<GetUserShelters, IEnumerable<ShelterBasicDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUserSheltersHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<ShelterBasicDto>> HandleAsync(GetUserShelters query)
        {
            List<ShelterDocument> userShelters = await GetUserSheltersAsync(query);
            
            return userShelters.Select(x => x.AsDto());
        }

        private async Task<List<ShelterDocument>> GetUserSheltersAsync(GetUserShelters query)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.Shelters)
            {
                Query = new MatchQuery
                {
                    Query = query.UserId.ToString(),
                    Field = Infer.Field<ShelterDocument>(p => p.Owners)
                }
            };

            ISearchResponse<ShelterDocument> userShelters = await _elasticClient.SearchAsync<ShelterDocument>(searchRequest);
            return userShelters?.Documents.ToList();
        }
    }
}