using System;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Queries.Shelters
{
    public class GetShelterViewsCount : IQuery<ShelterViewsDto>
    {
        public Guid ShelterId { get; set; }
    }
}