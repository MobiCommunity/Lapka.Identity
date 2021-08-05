using System.Collections;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;


namespace Lapka.Identity.Application.Queries
{
    public class GetShelters : IQuery<IEnumerable<ShelterDto>>
    {
        
    }
}