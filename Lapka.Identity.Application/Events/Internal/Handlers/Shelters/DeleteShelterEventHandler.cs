using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;

namespace Lapka.Identity.Application.Events.Internal.Handlers.Shelters
{
    public class DeleteShelterEventHandler : IDomainEventHandler<ShelterDeleted>
    {
        private readonly IShelterElasticsearchUpdater _elasticsearchUpdater;
        private readonly IShelterViewsRepository _viewsRepository;

        public DeleteShelterEventHandler(IShelterElasticsearchUpdater elasticsearchUpdater,
            IShelterViewsRepository viewsRepository)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
            _viewsRepository = viewsRepository;
        }

        public async Task HandleAsync(ShelterDeleted @event)
        {
            await _elasticsearchUpdater.DeleteDataAsync(@event.Shelter.Id.Value);
            await _viewsRepository.DeleteAsync(@event.Shelter.Id.Value);
        }
    }
}