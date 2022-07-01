using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Queries.Users;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries
{
    public class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public GetUserHandler(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<UserDto> HandleAsync(GetUser query)
        {
            UserDocument user = await GetUserAsync(query);

            return user.AsDto();
        }

        private async Task<UserDocument> GetUserAsync(GetUser query)
        {
            UserDocument user = await _repository.GetAsync(query.Id);
            if (user is null)
            {
                throw new UserNotFoundException(query.Id.ToString());
            }
            
            return user;
        }
    }
}