using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

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
            Shelter shelter = await GetShelterAsync(command);
            ValidIfUserCanManageShelter(command, shelter);

            shelter.Update(command.Name, new PhoneNumber(command.PhoneNumber), new EmailAddress(command.Email), new BankNumber(command.BankNumber));

            await _shelterRepository.UpdateAsync(shelter);

            await _eventProcessor.ProcessAsync(shelter.Events);
        }

        private static void ValidIfUserCanManageShelter(UpdateShelter command, Shelter shelter)
        {
            if (!shelter.Owners.Contains(command.UserAuth.UserId) && command.UserAuth.Role != "admin")
            {
                throw new UnauthorizedAccessException();
            }
        }

        private async Task<Shelter> GetShelterAsync(UpdateShelter command)
        {
            Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(command.Id.ToString());
            }

            return shelter;
        }
    }
}