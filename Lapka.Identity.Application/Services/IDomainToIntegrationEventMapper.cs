using Convey.CQRS.Events;
using System.Collections.Generic;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Application.Services
{
    public interface IDomainToIntegrationEventMapper
    {
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
        IEvent Map(IDomainEvent @event);
        
    }
}