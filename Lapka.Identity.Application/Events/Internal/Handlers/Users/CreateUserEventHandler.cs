using System.Threading.Tasks;
using Lapka.Identity.Application.Events.Abstract;
using Lapka.Identity.Application.Services.Elastic;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Users;

namespace Lapka.Identity.Application.Events.Internal.Handlers.Users
{
    public class CreateUserEventHandler : IDomainEventHandler<UserCreated>
    {
        private readonly IUserElasticsearchUpdater _elasticsearchUpdater;

        public CreateUserEventHandler(IUserElasticsearchUpdater elasticsearchUpdater)
        {
            _elasticsearchUpdater = elasticsearchUpdater;
        }
        public async Task HandleAsync(UserCreated @event)
        {
            await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.User);
        }
    }
}