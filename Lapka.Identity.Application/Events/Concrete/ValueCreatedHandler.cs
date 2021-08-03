using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;

namespace Lapka.Identity.Application.Events.Concrete
{
    public class ValueCreatedHandler : IDomainEventHandler<ValueCreated>
    {

        public Task HandleAsync(ValueCreated @event)
        {
            Console.WriteLine($"i caught {@event.Value.Name}");
            return Task.CompletedTask;
        }
    }
}