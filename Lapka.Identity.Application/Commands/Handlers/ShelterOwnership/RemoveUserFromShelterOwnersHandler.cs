using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
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
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;

        public RemoveUserFromShelterOwnersHandler(IEventProcessor eventProcessor, IUserRepository userRepository,
            IShelterRepository shelterRepository, IMessageBroker messageBroker,
            IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _shelterRepository = shelterRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(RemoveUserFromShelterOwners command)
        {
            User user = await GetUserAsync(command);
            Shelter shelter = await GetShelterAsync(command);

            shelter.RemoveOwner(user.Id.Value);

            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(shelter.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        private async Task<Shelter> GetShelterAsync(RemoveUserFromShelterOwners command)
        {
            Shelter shelter = await _shelterRepository.GetByIdAsync(command.ShelterId);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(command.ShelterId.ToString());
            }

            return shelter;
        }

        private async Task<User> GetUserAsync(RemoveUserFromShelterOwners command)
        {
            User user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId.ToString());
            }

            return user;
        }
    }
}