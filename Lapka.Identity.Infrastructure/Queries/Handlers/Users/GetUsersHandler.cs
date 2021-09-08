using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers.Users
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
            IEnumerable<UserDocument> users = await _repository.FindAsync(_ => true);

            return users.Select(x => x.AsDto());
        }
    }
}