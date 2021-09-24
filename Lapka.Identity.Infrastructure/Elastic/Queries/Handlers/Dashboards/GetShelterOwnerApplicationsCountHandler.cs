using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Elasticsearch.Net;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Dashboards
{
    public class GetShelterOwnerApplicationsCountHandler : IQueryHandler<GetShelterOwnerApplicationsCount, long>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterOwnerApplicationsCountHandler(IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<long> HandleAsync(GetShelterOwnerApplicationsCount query)
        {
            CountResponse countResponse = await _elasticClient.CountAsync<ShelterOwnerApplicationDocument>(s => s.Query(q => new QueryContainer(
                    new BoolQuery
                    {
                        Must = new List<QueryContainer>
                        {
                            new QueryContainer(new MatchPhraseQuery
                            {
                                Query = query.ShelterId.ToString(),
                                Field = Infer.Field<ShelterOwnerApplicationDocument>(p => p.ShelterId)
                            }),
                            new QueryContainer(new TermQuery()
                            {
                                Value = OwnerApplicationStatus.Pending,
                                Field = Infer.Field<ShelterOwnerApplicationDocument>(p => p.Status)
                            }),
                        }
                    })).Index(_elasticSearchOptions.Aliases.ShelterOwnerApplications));
            
            if (countResponse is null)
            {
                throw new ElasticsearchClientException("Could not get shelter owner application documents");
            }
            
            return countResponse.Count;
        }
    }
}