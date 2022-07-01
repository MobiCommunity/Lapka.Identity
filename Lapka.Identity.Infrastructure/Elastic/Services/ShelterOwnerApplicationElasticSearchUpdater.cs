// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Lapka.Identity.Application.Services;
// using Lapka.Identity.Application.Services.Elastic;
// using Lapka.Identity.Core.Entities;
// using Lapka.Identity.Core.ValueObjects;
// using Lapka.Identity.Infrastructure.Elastic.Options;
// using Lapka.Identity.Infrastructure.Mongo.Documents;
// using Lapka.Identity.Infrastructure.Options;
// using Microsoft.Extensions.Logging;
// using Nest;
//
// namespace Lapka.Identity.Infrastructure.Elastic.Services
// {
//     public class ShelterOwnerApplicationElasticSearchUpdater : IShelterOwnerApplicationElasticSearchUpdater
//     {
//         private readonly ILogger<ShelterOwnerApplicationElasticSearchUpdater> _logger;
//         private readonly IElasticClient _elasticClient;
//         private readonly ElasticSearchOptions _elasticSearchOptions;
//
//         public ShelterOwnerApplicationElasticSearchUpdater(ILogger<ShelterOwnerApplicationElasticSearchUpdater> logger,
//             IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
//         {
//             _logger = logger;
//             _elasticClient = elasticClient;
//             _elasticSearchOptions = elasticSearchOptions;
//         }
//
//         public async Task InsertAndUpdateDataAsync(ShelterOwnerApplication application)
//         {
//             IndexResponse response = await _elasticClient.IndexAsync(application.AsDocument(),
//                 x => x.Index(_elasticSearchOptions.Aliases.ShelterOwnerApplications));
//
//             if (!response.IsValid)
//             {
//                 _logger.LogError("Error occured when trying to insert or update" +
//                                  $" shelter owner application {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
//             }
//         }
//
//         public async Task DeleteDataAsync(Guid applicationId)
//         {
//             DeleteResponse response = await _elasticClient.DeleteAsync<ShelterOwnerApplicationDocument>(applicationId,
//                 x => x.Index(_elasticSearchOptions.Aliases.ShelterOwnerApplications));
//
//             if (!response.IsValid)
//             {
//                 _logger.LogError("Error occured when trying to delete" +
//                                  $" shelter owner application {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
//             }
//         }
//     }
// }