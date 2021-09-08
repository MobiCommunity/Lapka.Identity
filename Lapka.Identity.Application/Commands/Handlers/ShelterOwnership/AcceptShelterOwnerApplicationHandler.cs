using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Handlers.ShelterOwnership
{
    public class AcceptShelterOwnerApplicationHandler : ICommandHandler<AcceptShelterOwnerApplication>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterOwnerApplicationRepository _shelterOwnerApplicationRepository;
        private readonly IShelterRepository _shelterRepository;
        private readonly IUserRepository _userRepository;

        public AcceptShelterOwnerApplicationHandler(IEventProcessor eventProcessor, IUserRepository userRepository,
            IShelterRepository shelterRepository, IShelterOwnerApplicationRepository shelterOwnerApplicationRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _shelterRepository = shelterRepository;
            _shelterOwnerApplicationRepository = shelterOwnerApplicationRepository;
        }

        public async Task HandleAsync(AcceptShelterOwnerApplication command)
        {
            ShelterOwnerApplication application = await _shelterOwnerApplicationRepository.GetAsync(command.Id);
            if (application is null) throw new ShelterOwnerApplicationNotFoundException(command.Id.ToString());
            
            if (application.Status != OwnerApplicationStatus.Pending)
                throw new OwnerApplicationStatusHasToBePendingException(application.Id.ToString(), application.Status);

            User user = await _userRepository.GetAsync(application.UserId);
            if (user is null) throw new UserNotFoundException(application.UserId.ToString());

            Shelter shelter = await _shelterRepository.GetByIdAsync(application.ShelterId);
            if (shelter is null) throw new ShelterNotFoundException(application.ShelterId.ToString());

            application.AcceptApplication();
            shelter.AddOwner(application.UserId);

            await _shelterOwnerApplicationRepository.UpdateAsync(application);
            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}