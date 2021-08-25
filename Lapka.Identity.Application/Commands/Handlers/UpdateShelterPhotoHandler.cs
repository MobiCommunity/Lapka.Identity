using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class UpdateShelterPhotoHandler : ICommandHandler<UpdateShelterPhoto>
    {
        private readonly ILogger<UpdateShelterPhotoHandler> _logger;
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IEventProcessor _eventProcessor;

        public UpdateShelterPhotoHandler(ILogger<UpdateShelterPhotoHandler> logger, IShelterRepository shelterRepository,
            IGrpcPhotoService grpcPhotoService, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _shelterRepository = shelterRepository;
            _grpcPhotoService = grpcPhotoService;
            _eventProcessor = eventProcessor;
        }

        public async Task HandleAsync(UpdateShelterPhoto command)
        {
            string photoPath =  $"{command.PhotoId:N}.{command.Photo.GetFileExtension()}";
            Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);
            string oldPhotoPath = shelter.PhotoPath;
            
            shelter.UpdatePhoto(photoPath);

            try
            {
                await _grpcPhotoService.DeleteAsync(oldPhotoPath, BucketName.ShelterPhotos);
                await _grpcPhotoService.AddAsync(photoPath, command.Photo.Content, BucketName.ShelterPhotos);
            }
            catch(Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
            
            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}