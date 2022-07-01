// using System.Threading.Tasks;
// using Lapka.Identity.Application.Events.Abstract;
// using Lapka.Identity.Application.Services.Elastic;
// using Lapka.Identity.Core.Events.Concrete;
// using Lapka.Identity.Core.Events.Concrete.Users;
//
// namespace Lapka.Identity.Application.Events.Internal.Handlers.Users
// {
//     public class DeleteUserEventHandler : IDomainEventHandler<UserDeleted>
//     {
//         private readonly IUserElasticsearchUpdater _elasticsearchUpdater;
//
//         public DeleteUserEventHandler(IUserElasticsearchUpdater elasticsearchUpdater)
//         {
//             _elasticsearchUpdater = elasticsearchUpdater;
//         }
//         public async Task HandleAsync(UserDeleted @event)
//         {
//             await _elasticsearchUpdater.DeleteDataAsync(@event.User.Id.Value);
//         }
//     }
// }