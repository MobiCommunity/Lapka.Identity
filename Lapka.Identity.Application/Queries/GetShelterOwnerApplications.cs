﻿using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries
{
    public class GetShelterOwnerApplications : IQuery<IEnumerable<ShelterOwnerApplicationDto>>
    {
        
    }
}