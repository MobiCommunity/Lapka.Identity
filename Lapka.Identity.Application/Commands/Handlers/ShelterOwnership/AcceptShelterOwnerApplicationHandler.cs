using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Ownership;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Handlers.ShelterOwnership
{
    public class AcceptShelterOwnerApplicationHandler : ICommandHandler<AcceptShelterOwnerApplication>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterOwnerApplicationRepository _shelterOwnerApplicationRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;
        private readonly IShelterRepository _shelterRepository;
        private readonly IUserRepository _userRepository;

        public AcceptShelterOwnerApplicationHandler(IEventProcessor eventProcessor, IUserRepository userRepository,
            IShelterRepository shelterRepository, IShelterOwnerApplicationRepository shelterOwnerApplicationRepository,
            IMessageBroker messageBroker, IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _shelterRepository = shelterRepository;
            _shelterOwnerApplicationRepository = shelterOwnerApplicationRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(AcceptShelterOwnerApplication command)
        {
            ShelterOwnerApplication application = await GetShelterOwnerApplicationAsync(command);
            ValidApplicationStatus(application);
            
            User user = await GetUserAsync(application);
            Shelter shelter = await GetShelterAsync(application);

            application.AcceptApplication();
            shelter.AddOwner(user.Id.Value);

            await _shelterOwnerApplicationRepository.UpdateAsync(application);
            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
            await _eventProcessor.ProcessAsync(application.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(shelter.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        private async Task<User> GetUserAsync(ShelterOwnerApplication application)
        {
            User user = await _userRepository.GetAsync(application.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(application.UserId.ToString());
            }

            return user;
        }

        private async Task<Shelter> GetShelterAsync(ShelterOwnerApplication application)
        {
            Shelter shelter = await _shelterRepository.GetByIdAsync(application.ShelterId);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(application.ShelterId.ToString());
            }

            return shelter;
        }

        private static void ValidApplicationStatus(ShelterOwnerApplication application)
        {
            if (application.Status != OwnerApplicationStatus.Pending)
            {
                throw new OwnerApplicationStatusHasToBePendingException(application.Id.ToString(), application.Status);
            }
        }

        private async Task<ShelterOwnerApplication> GetShelterOwnerApplicationAsync(AcceptShelterOwnerApplication command)
        {
            ShelterOwnerApplication application = await _shelterOwnerApplicationRepository.GetAsync(command.Id);
            if (application is null)
            {
                throw new ShelterOwnerApplicationNotFoundException(command.Id.ToString());
            }

            return application;
        }
    }
}