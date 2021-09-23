using System;
using System.Collections;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetUserPhoto : IQuery<string>
    {
        public Guid Id { get; set; }
    }
}