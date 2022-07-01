// using System.Threading.Tasks;
// using Lapka.Identity.Application.Events.Abstract;
// using Lapka.Identity.Application.Services.Elastic;
// using Lapka.Identity.Core.Events.Concrete.Shelters;
//
// namespace Lapka.Identity.Application.Events.Internal.Handlers.Shelters
// {
//     public class ShelterOwnerAddedEventHandler : IDomainEventHandler<ShelterOwnerAdded>
//     {
//         private readonly IShelterElasticsearchUpdater _elasticsearchUpdater;
//
//         public ShelterOwnerAddedEventHandler(IShelterElasticsearchUpdater elasticsearchUpdater)
//         {
//             _elasticsearchUpdater = elasticsearchUpdater;
//         }
//         public async Task HandleAsync(ShelterOwnerAdded @event)
//         {
//             await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Shelter);
//         }
//     }
// }