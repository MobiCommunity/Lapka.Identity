using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries.Users
{
    public class GetUsers : IQuery<IEnumerable<UserDto>>
    {
        
    }
}