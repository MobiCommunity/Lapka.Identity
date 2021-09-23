using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
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
            User user = await GetUserAsync(command);
            Shelter shelter = await GetShelterAsync(command);

            await ValidifUserHasNoPendingApplication(command, user, shelter);
            ValidIfUserIsNotOwnerOfShelter(shelter, user);

            ShelterOwnerApplication application = ShelterOwnerApplication.Create(command.Id, command.ShelterId,
                command.UserId, OwnerApplicationStatus.Pending, DateTime.UtcNow);

            await _shelterOwnerApplicationRepository.AddApplicationAsync(application);
            await _eventProcessor.ProcessAsync(application.Events);
        }

        private static void ValidIfUserIsNotOwnerOfShelter(Shelter shelter, User user)
        {
            if (shelter.Owners.Any(x => x == user.Id.Value))
            {
                throw new UserAlreadyIsOwnerOfShelterException(shelter.Id.Value, user.Id.Value);
            }
        }

        private async Task ValidifUserHasNoPendingApplication(AddShelterOwnerApplication command, User user,
            Shelter shelter)
        {
            IEnumerable<ShelterOwnerApplication> applications =
                await _shelterOwnerApplicationRepository.GetAllAsync(command.UserId, command.ShelterId);

            if (applications.Any(x => x.Status == OwnerApplicationStatus.Pending))
            {
                throw new ApplicationForShelterOwnerIsAlreadyMadeException(user.Id.Value.ToString(),
                    shelter.Id.Value.ToString());
            }
        }

        private async Task<Shelter> GetShelterAsync(AddShelterOwnerApplication command)
        {
            Shelter shelter = await _shelterRepository.GetByIdAsync(command.ShelterId);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(command.ShelterId.ToString());
            }

            return shelter;
        }

        private async Task<User> GetUserAsync(AddShelterOwnerApplication command)
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