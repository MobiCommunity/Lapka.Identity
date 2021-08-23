using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions.User;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
{
    public class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto> HandleAsync(GetUser query)
        {
            User user = await _userRepository.GetAsync(query.Id);

            if (user is null)
            {
                throw new UserNotFoundException(user.Id.ToString());
            }

            return user.AsDto();
        }
    }
}