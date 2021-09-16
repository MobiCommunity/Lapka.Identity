using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;

namespace Lapka.Identity.Application.Events.Concrete
{
    public class ShelterUpdatedHandler  : IDomainEventHandler<ShelterUpdated>
    {
        public Task HandleAsync(ShelterUpdated @event)
        {
            Console.WriteLine($"i caught {@event.Shelter.Name}");
            return Task.CompletedTask;
        }
    }
}