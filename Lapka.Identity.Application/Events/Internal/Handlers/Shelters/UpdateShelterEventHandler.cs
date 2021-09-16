using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;

namespace Lapka.Identity.Application.Events.Internal.Handlers.Shelters
{
    public class UpdateShelterEventHandler : IDomainEventHandler<ShelterUpdated>
    {
        private readonly IShelterElasticsearchUpdater _elasticsearchUpdater;

        public UpdateShelterEventHandler(IShelterElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(ShelterUpdated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Shelter);
        }
    }
}