using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers.Shelter
{
    public class DeleteShelterHandler : ICommandHandler<DeleteShelter>
    {
        private readonly ILogger<DeleteShelterHandler> _logger;
        private readonly IShelterRepository _shelterRepository;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _photoService;

        public DeleteShelterHandler(ILogger<DeleteShelterHandler> logger, IShelterRepository shelterRepository,
            IEventProcessor eventProcessor, IGrpcPhotoService photoService)
        {
            _logger = logger;
            _shelterRepository = shelterRepository;
            _eventProcessor = eventProcessor;
            _photoService = photoService;
        }

        public async Task HandleAsync(DeleteShelter command)
        {
            Core.Entities.Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);
            if (shelter is null)
            {
                throw new ShelterNotFoundException();
            }

            shelter.Delete();
            try
            {
                await _photoService.DeleteAsync(shelter.PhotoId, BucketName.ShelterPhotos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Did not deleted shelter photo with id: {shelter.PhotoId}");
            }

            await _shelterRepository.DeleteAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}