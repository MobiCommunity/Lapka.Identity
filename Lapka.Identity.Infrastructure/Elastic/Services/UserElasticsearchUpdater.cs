// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Lapka.Identity.Application.Dto;
// using Lapka.Identity.Application.Services.Elastic;
// using Lapka.Identity.Core.Entities;
// using Lapka.Identity.Infrastructure.Elastic.Options;
// using Lapka.Identity.Infrastructure.Mongo.Documents;
// using Lapka.Identity.Infrastructure.Options;
// using Microsoft.Extensions.Logging;
// using Nest;
//
// namespace Lapka.Identity.Infrastructure.Elastic.Services
// {
//     public class UserElasticsearchUpdater : IUserElasticsearchUpdater
//     {
//         private readonly ILogger<UserElasticsearchUpdater> _logger;
//         private readonly IElasticClient _elasticClient;
//         private readonly ElasticSearchOptions _elasticSearchOptions;
//
//         public UserElasticsearchUpdater(ILogger<UserElasticsearchUpdater> logger, IElasticClient elasticClient,
//             ElasticSearchOptions elasticSearchOptions)
//         {
//             _logger = logger;
//             _elasticClient = elasticClient;
//             _elasticSearchOptions = elasticSearchOptions;
//         }
//         
//         public async Task InsertAndUpdateDataAsync(User user)
//         {
//             IndexResponse response = await _elasticClient.IndexAsync(user.AsDto(),
//                 x => x.Index(_elasticSearchOptions.Aliases.Users));
//
//             if (!response.IsValid)
//             {
//                 _logger.LogError("Error occured when trying to insert or update" +
//                                  $" user {response.ServerError.Error.Reason}");
//             }
//         }
//
//         public async Task DeleteDataAsync(Guid userId)
//         {
//             DeleteResponse response = await _elasticClient.DeleteAsync<UserDto>(userId,
//                 x => x.Index(_elasticSearchOptions.Aliases.Users));
//
//             if (!response.IsValid)
//             {
//                 _logger.LogError("Error occured when trying to delete" +
//                                  $" user {response.ServerError.Error.Headers.Values.FirstOrDefault()}");
//             }
//         }
//     }
// }