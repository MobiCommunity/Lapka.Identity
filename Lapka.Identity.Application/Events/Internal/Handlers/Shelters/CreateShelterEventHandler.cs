using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;

namespace Lapka.Identity.Application.Events.Internal.Handlers.Shelters
{
    public class CreateShelterEventHandler : IDomainEventHandler<ShelterCreated>
    {
        private readonly IShelterElasticsearchUpdater _elasticsearchUpdater;

        public CreateShelterEventHandler(IShelterElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(ShelterCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Shelter);
        }
    }
}