using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
{
    public class GetUsersHandler : IQueryHandler<GetUsers, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query)
        {
            IEnumerable<User> users = await _userRepository.GetAllAsync();

            return users.Select(x => x.AsDto());
        }
    }
}