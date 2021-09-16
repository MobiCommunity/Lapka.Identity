using System;
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
    public class AddShelterOwnerApplicationHandler : ICommandHandler<AddShelterOwnerApplication>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterOwnerApplicationRepository _shelterOwnerApplicationRepository;
        private readonly IShelterRepository _shelterRepository;
        private readonly IUserRepository _userRepository;

        public AddShelterOwnerApplicationHandler(IEventProcessor eventProcessor, IUserRepository userRepository,
            IShelterRepository shelterRepository, IShelterOwnerApplicationRepository shelterOwnerApplicationRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _shelterRepository = shelterRepository;
            _shelterOwnerApplicationRepository = shelterOwnerApplicationRepository;
        }

        public async Task HandleAsync(AddShelterOwnerApplication command)
        {
            User user = await _userRepository.GetAsync(command.UserId);
            if (user is null) throw new UserNotFoundException(command.UserId.ToString());

            Shelter shelter = await _shelterRepository.GetByIdAsync(command.ShelterId);
            if (shelter is null) throw new ShelterNotFoundException(command.ShelterId.ToString());

            ShelterOwnerApplication application =
                await _shelterOwnerApplicationRepository.GetAsync(command.UserId, command.ShelterId);

            if (application is { })
                throw new ApplicationForShelterOwnerIsAlreadyMadeException(user.Id.Value.ToString(),
                    shelter.Id.Value.ToString());

            application = new ShelterOwnerApplication(command.Id, command.ShelterId, command.UserId,
                OwnerApplicationStatus.Pending, DateTime.UtcNow);

            await _shelterOwnerApplicationRepository.AddApplicationAsync(application);
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}