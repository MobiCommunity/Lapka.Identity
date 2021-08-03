using System.Threading.Tasks;
using Lapka.Identity.Core.Events.Abstract;

namespace Lapka.Identity.Application.Events.Abstract
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T @event);
    }
}