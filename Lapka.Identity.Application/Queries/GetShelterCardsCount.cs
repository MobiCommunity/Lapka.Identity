using System;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;

namespace Lapka.Identity.Application.Queries
{
    public class GetShelterCardsCount : IQuery<int>
    {
        public UserAuth Auth { get; set; }
        public Guid ShelterId { get; set; }
    }
}