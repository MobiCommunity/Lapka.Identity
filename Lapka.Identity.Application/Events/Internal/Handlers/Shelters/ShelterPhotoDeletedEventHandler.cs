// using System.Threading.Tasks;
// using Lapka.Identity.Application.Events.Abstract;
// using Lapka.Identity.Application.Services.Elastic;
// using Lapka.Identity.Core.Events.Concrete;
// using Lapka.Identity.Core.Events.Concrete.Shelters;
//
// namespace Lapka.Identity.Application.Events.Internal.Handlers.Shelters
// {
//     public class ShelterPhotoDeletedEventHandler : IDomainEventHandler<ShelterPhotoUpdated>
//     {
//         private readonly IShelterElasticsearchUpdater _elasticsearchUpdater;
//
//         public ShelterPhotoDeletedEventHandler(IShelterElasticsearchUpdater elasticsearchUpdater)
//         {
//             _elasticsearchUpdater = elasticsearchUpdater;
//         }
//         public async Task HandleAsync(ShelterPhotoUpdated @event)
//         {
//             await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Shelter);
//         }
//     }
// }