using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;

namespace Lapka.Identity.Application.Events.Concrete
{
    public class ShelterUpdatedHandler  : IDomainEventHandler<ShelterCreated>
    {
        public Task HandleAsync(ShelterCreated @event)
        {
            Console.WriteLine($"i caught {@event.Shelter.Name}");
            return Task.CompletedTask;
        }
    }
}