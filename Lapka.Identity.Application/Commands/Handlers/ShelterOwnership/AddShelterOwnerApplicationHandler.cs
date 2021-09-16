using System;
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
            ShelterOwnerApplication application = await CreateShelterOwnerApplicationAsync(command, user, shelter);

            await _shelterOwnerApplicationRepository.AddApplicationAsync(application);
            await _eventProcessor.ProcessAsync(shelter.Events);
            await _eventProcessor.ProcessAsync(application.Events);
        }

        private async Task<ShelterOwnerApplication> CreateShelterOwnerApplicationAsync(
            AddShelterOwnerApplication command, User user, Shelter shelter)
        {
            ShelterOwnerApplication application =
                await _shelterOwnerApplicationRepository.GetAsync(command.UserId, command.ShelterId);

            if (application is { })
            {
                throw new ApplicationForShelterOwnerIsAlreadyMadeException(user.Id.Value.ToString(),
                    shelter.Id.Value.ToString());
            }

            application = ShelterOwnerApplication.Create(command.Id, command.ShelterId, command.UserId,
                OwnerApplicationStatus.Pending, DateTime.UtcNow);
            return application;
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