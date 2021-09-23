using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Dashboards;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Handlers.Dashboard
{
    public class IncrementShelterViewsHandler : ICommandHandler<IncrementShelterViews>
    {
        private readonly IShelterViewsRepository _shelterViewsRepository;
        private readonly IShelterRepository _shelterRepository;

        public IncrementShelterViewsHandler(IShelterViewsRepository shelterViewsRepository,
            IShelterRepository shelterRepository)
        {
            _shelterViewsRepository = shelterViewsRepository;
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(IncrementShelterViews command)
        {
            ShelterViews shelterViews = await _shelterViewsRepository.GetByIdAsync(command.ShelterId);
            if (shelterViews is null)
            {
                Shelter shelter = await _shelterRepository.GetByIdAsync(command.ShelterId);
                if (shelter is null)
                {
                    throw new ShelterNotFoundException(shelter.Id.Value.ToString());
                }

                await _shelterViewsRepository.AddAsync(new ShelterViews(shelterViews.Id, 0, new List<ViewHistory>()));
                shelterViews = await _shelterViewsRepository.GetByIdAsync(command.ShelterId);
            }

            shelterViews.IncreaseViewsCount();

            await _shelterViewsRepository.UpdateAsync(shelterViews);
        }
    }
}