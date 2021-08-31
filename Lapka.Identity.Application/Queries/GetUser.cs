using System;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public Guid Id { get; set; }
    }
}