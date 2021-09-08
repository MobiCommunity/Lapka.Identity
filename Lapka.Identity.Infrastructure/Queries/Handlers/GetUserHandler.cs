using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
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
            UserDocument user = await _repository.GetAsync(query.Id);

            if (user is null)
            {
                throw new UserNotFoundException(query?.Id.ToString());
            }

            return user.AsDto();
        }
    }
}