using System;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Lapka.Identity.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Services
{
    public class ShelterElasticsearchUpdater : IShelterElasticsearchUpdater
    {
        private readonly ILogger<ShelterElasticsearchUpdater> _logger;
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public ShelterElasticsearchUpdater(ILogger<ShelterElasticsearchUpdater> logger, IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task InsertAndUpdateDataAsync(Shelter shelter)
        {
            IndexResponse response = await _elasticClient.IndexAsync(shelter.AsDocument(),
                x => x.Index(_elasticSearchOptions.Aliases.Shelters));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to insert or update" +
                                 $" shelter {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }

        public async Task DeleteDataAsync(Guid shelterId)
        {
            DeleteResponse response = await _elasticClient.DeleteAsync<ShelterDocument>(shelterId,
                x => x.Index(_elasticSearchOptions.Aliases.Shelters));

            if (!response.IsValid)
            {
                _logger.LogError("Error occured when trying to delete" +
                                 $" shelter {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
            }
        }
    }
}