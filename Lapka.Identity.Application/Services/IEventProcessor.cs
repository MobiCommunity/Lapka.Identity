using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Application.Services
{
    public interface IEventProcessor
    {
        Task ProcessAsync(IEnumerable<IDomainEvent> events);
    }
}