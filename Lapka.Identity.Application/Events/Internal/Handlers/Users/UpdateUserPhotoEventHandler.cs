using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Users;

namespace Lapka.Identity.Application.Events.Internal.Handlers.Users
{
    public class UpdateUserPhotoEventHandler : IDomainEventHandler<UserPhotoUpdated>
    {
        private readonly IUserElasticsearchUpdater _elasticsearchUpdater;

        public UpdateUserPhotoEventHandler(IUserElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(UserPhotoUpdated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.User);
        }
    }
}