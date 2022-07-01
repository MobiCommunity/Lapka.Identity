// using System.Threading.Tasks;
// using Lapka.Identity.Application.Events.Abstract;
// using Lapka.Identity.Application.Services.Elastic;
// using Lapka.Identity.Core.Events.Concrete;
// using Lapka.Identity.Core.Events.Concrete.Applications;
// using Lapka.Identity.Core.Events.Concrete.Shelters;
//
// namespace Lapka.Identity.Application.Events.Internal.Handlers.Applications
// {
//     public class CreateApplicationEventHandler : IDomainEventHandler<CreatedShelterOwnerApplication>
//     {
//         private readonly IShelterOwnerApplicationElasticSearchUpdater _elasticsearchUpdater;
//
//         public CreateApplicationEventHandler(IShelterOwnerApplicationElasticSearchUpdater elasticsearchUpdater)
//         {
//             _elasticsearchUpdater = elasticsearchUpdater;
//         }
//         public async Task HandleAsync(CreatedShelterOwnerApplication @event)
//         {
//             await _elasticsearchUpdater.InsertAndUpdateDataAsync(@event.Application);
//         }
//     }
// }