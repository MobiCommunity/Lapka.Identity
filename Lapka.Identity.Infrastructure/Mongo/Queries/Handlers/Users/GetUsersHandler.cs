using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Users;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries.Handlers.Users
{
    public class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetUsersHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query)
        {
            IEnumerable<UserDto> users = await GetUsersAsync();
            
            return users;
        }

        private async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.Users);

            ISearchResponse<UserDto>
                usersResponse = await _elasticClient.SearchAsync<UserDto>(searchRequest);
            
             return usersResponse?.Documents.AsEnumerable();
        }
    }
}