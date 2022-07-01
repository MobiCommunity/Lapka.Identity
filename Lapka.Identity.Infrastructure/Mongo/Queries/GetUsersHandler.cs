using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries.Users;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries
{
    public class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public GetUsersHandler(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query)
        {
            IEnumerable<UserDto> users = await GetUsersAsync();
            
            return users;
        }

        private async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            IReadOnlyList<UserDocument> user = await _repository.FindAsync(_ => true);

            return user.Select(u => u.AsDto());
        }
    }
}