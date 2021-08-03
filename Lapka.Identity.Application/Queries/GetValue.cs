using Convey.CQRS.Queries;
using System;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries
{
    public class GetValue : IQuery<ValueDto>
    {
        public Guid Id { get; set; }

    }
}
