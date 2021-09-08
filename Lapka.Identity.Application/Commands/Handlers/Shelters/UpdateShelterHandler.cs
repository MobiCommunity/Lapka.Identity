using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;

namespace Lapka.Identity.Application.Commands.Handlers.Shelters
{
    public class UpdateShelterHandler : ICommandHandler<UpdateShelter>
    {
        private readonly IShelterRepository _shelterRepository;
        private readonly IEventProcessor _eventProcessor;

        public UpdateShelterHandler(IShelterRepository shelterRepository, IEventProcessor eventProcessor)
        {
            _shelterRepository = shelterRepository;
            _eventProcessor = eventProcessor;
        }


        public async Task HandleAsync(UpdateShelter command)
        {
            Core.Entities.Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(command.Id.ToString());
            }

            if (shelter.Owners.Any(x => x != command.UserAuth.UserId) && command.UserAuth.Role != "admin")
            {
                throw new UnauthorizedAccessException();
            }

            shelter.Update(command.Name, command.Address, command.GeoLocation, command.PhoneNumber, command.Email,
                command.BankNumber);

            await _shelterRepository.UpdateAsync(shelter);

            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}