using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;

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
                throw new UserNotFoundException(query?.Id.ToString());
            }

            return user.AsDto();
        }
    }
}