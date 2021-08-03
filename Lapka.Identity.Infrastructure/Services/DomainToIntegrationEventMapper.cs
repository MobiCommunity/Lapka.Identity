using Convey.CQRS.Events;
using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Infrastructure.Services
{
    public class DomainToIntegrationEventMapper : IDomainToIntegrationEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);

        public IEvent Map(IDomainEvent @event) => @event switch
        {
            _ => null
        };
    }
}