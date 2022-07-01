// using System.Threading.Tasks;
// using Convey.CQRS.Queries;
// using Lapka.Identity.Application.Dto;
// using Lapka.Identity.Application.Exceptions.Users;
// using Lapka.Identity.Application.Queries.Users;
// using Lapka.Identity.Infrastructure.Elastic.Options;
// using Lapka.Identity.Infrastructure.Mongo.Documents;
// using Nest;
//
// namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Users
// {
//     public class GetUserHandler : IQueryHandler<GetUser, UserDto>
//     {
//         private readonly IElasticClient _elasticClient;
//         private readonly ElasticSearchOptions _elasticSearchOptions;
//
//         public GetUserHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
//         {
//             _elasticClient = elasticClient;
//             _elasticSearchOptions = elasticSearchOptions;
//         }
//         
//         public async Task<UserDto> HandleAsync(GetUser query)
//         {
//             UserDocument user = await GetUserAsync(query);
//
//             return user.AsDto();
//         }
//
//         private async Task<UserDocument> GetUserAsync(GetUser query)
//         {
//             GetResponse<UserDocument> response = await _elasticClient.GetAsync(
//                 new DocumentPath<UserDocument>(new Id(query.Id.ToString())),
//                 x => x.Index(_elasticSearchOptions.Aliases.Users));
//
//             UserDocument user = response?.Source;
//             if (user is null)
//             {
//                 throw new UserNotFoundException(query.Id.ToString());
//             }
//
//             return user;
//         }
//     }
// }