using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers.Shelters
{
    public class DeleteShelterHandler : ICommandHandler<DeleteShelter>
    {
        private readonly ILogger<DeleteShelterHandler> _logger;
        private readonly IShelterRepository _shelterRepository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _photoService;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;

        public DeleteShelterHandler(ILogger<DeleteShelterHandler> logger, IShelterRepository shelterRepository,
            IEventProcessor eventProcessor, IGrpcPhotoService photoService, IMessageBroker messageBroker,
            IDomainToIntegrationEventMapper eventMapper)
        {
            _logger = logger;
            _shelterRepository = shelterRepository;
            _eventProcessor = eventProcessor;
            _photoService = photoService;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(DeleteShelter command)
        {
            ValidUserRole(command);

            Shelter shelter = await GetShelterAsync(command);

            await DeletePhotoFromMinioAsync(command, shelter);
            
            shelter.Delete();

            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(shelter.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        private async Task DeletePhotoFromMinioAsync(DeleteShelter command, Shelter shelter)
        {
            try
            {
                if (!string.IsNullOrEmpty(shelter.PhotoPath))
                {
                    await _photoService.DeleteAsync(shelter.PhotoPath, command.UserAuth.UserId, BucketName.ShelterPhotos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Did not deleted shelter photo with id: {shelter.PhotoPath}");
            }
        }

        private async Task<Shelter> GetShelterAsync(DeleteShelter command)
        {
            Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);
            if (shelter is null)
            {
                throw new ShelterNotFoundException(command.Id.ToString());
            }

            return shelter;
        }

        private static void ValidUserRole(DeleteShelter command)
        {
            if (command.UserAuth.Role != "admin")
            {
                throw new Exceptions.UnauthorizedAccessException();
            }
        }
        
    }
}