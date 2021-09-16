using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Applications;
using Lapka.Identity.Core.Events.Concrete.Shelters;

namespace Lapka.Identity.Application.Events.Internal.Handlers.Applications
{
    public class UpdatedApplicationEventHandler : IDomainEventHandler<UpdatedShelterOwnerApplication>
    {
        private readonly IShelterOwnerApplicationElasticSearchUpdater _elasticsearchUpdater;

        public UpdatedApplicationEventHandler(IShelterOwnerApplicationElasticSearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(UpdatedShelterOwnerApplication @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Application);
        }
    }
}