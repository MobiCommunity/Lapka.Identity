using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class CreateShelterHandler : ICommandHandler<CreateShelter>
    {
        private readonly ILogger<CreateShelterHandler> _logger;
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IEventProcessor _eventProcessor;

        public CreateShelterHandler(ILogger<CreateShelterHandler> logger, IShelterRepository shelterRepository,
            IGrpcPhotoService grpcPhotoService, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _shelterRepository = shelterRepository;
            _grpcPhotoService = grpcPhotoService;
            _eventProcessor = eventProcessor;
        }

        public async Task HandleAsync(CreateShelter command)
        {
            string photoPath =  $"{command.PhotoId:N}.{command.Photo.GetFileExtension()}"; 

            Shelter created = Shelter.Create(command.Id, command.Name, command.Address, 
                command.GeoLocation, photoPath, command.PhoneNumber, command.Email);

            await _shelterRepository.AddAsync(created);
            
            try
            {
                await _grpcPhotoService.AddAsync(photoPath, command.Photo.Content, BucketName.ShelterPhotos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            
            await _eventProcessor.ProcessAsync(created.Events);
            
        }
    }
}