using Convey.CQRS.Events;
using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Events;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete.Shelters;

namespace Lapka.Identity.Infrastructure.Services
{
    public class DomainToIntegrationEventMapper : IDomainToIntegrationEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);

        public IEvent Map(IDomainEvent @event) => @event switch
        {
            ShelterCreated shelterCreated =>
                new ShelterAdded(shelterCreated.Shelter.Id.Value),
            
            ShelterOwnerRemoved shelterOwnerRemoved =>
                new ShelterOwnerUnassigned(shelterOwnerRemoved.Shelter.Id.Value, shelterOwnerRemoved.OwnerId),
            
            ShelterOwnerAdded shelterOwnerAdded =>
                new ShelterOwnerAssigned(shelterOwnerAdded.Shelter.Id.Value, shelterOwnerAdded.OwnerId),
            
            _ => null
        };
    }
}