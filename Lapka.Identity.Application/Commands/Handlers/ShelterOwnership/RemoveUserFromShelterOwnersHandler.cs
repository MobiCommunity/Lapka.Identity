using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Commands.Handlers.ShelterOwnership
{
    public class RemoveUserFromShelterOwnersHandler : ICommandHandler<RemoveUserFromShelterOwners>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;
        private readonly IShelterRepository _shelterRepository;

        public RemoveUserFromShelterOwnersHandler(IEventProcessor eventProcessor, IUserRepository userRepository,
            IShelterRepository shelterRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _shelterRepository = shelterRepository;
        }

        public async Task HandleAsync(RemoveUserFromShelterOwners command)
        {
            User user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId.ToString());
            }

            Shelter shelter = await _shelterRepository.GetByIdAsync(command.ShelterId);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(command.ShelterId.ToString());
            }
            
            shelter.RemoveOwner(command.UserId);

            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}