using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;

namespace Lapka.Identity.Application.Events.Internal.Handlers.Shelters
{
    public class DeleteShelterEventHandler : IDomainEventHandler<ShelterDeleted>
    {
        private readonly IShelterElasticsearchUpdater _elasticsearchUpdater;
        private readonly IShelterViewsRepository _viewsRepository;
        private readonly IShelterOwnerApplicationRepository _applicationRepository;

        public DeleteShelterEventHandler(IShelterElasticsearchUpdater elasticsearchUpdater,
            IShelterViewsRepository viewsRepository, IShelterOwnerApplicationRepository applicationRepository)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
            _viewsRepository = viewsRepository;
            _applicationRepository = applicationRepository;
        }

        public async Task HandleAsync(ShelterDeleted @event)
        {
            IEnumerable<ShelterOwnerApplication> shelterApplications =
                await _applicationRepository.GetApplicationsMadeForShelterAsync(@event.Shelter.Id.Value);

            foreach (ShelterOwnerApplication application in shelterApplications)
            {
                application.DeclineApplication();
                await _applicationRepository.UpdateAsync(application);
            }
            
            await _elasticsearchUpdater.DeleteDataAsync(@event.Shelter.Id.Value);
            await _viewsRepository.DeleteAsync(@event.Shelter.Id.Value);
        }
    }
}