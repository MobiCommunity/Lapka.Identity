using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Handlers.Shelter
{
    public class UpdateShelterPhotoHandler : ICommandHandler<UpdateShelterPhoto>
    {
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IEventProcessor _eventProcessor;

        public UpdateShelterPhotoHandler(IShelterRepository shelterRepository, IGrpcPhotoService grpcPhotoService,
            IEventProcessor eventProcessor)
        {
            _shelterRepository = shelterRepository;
            _grpcPhotoService = grpcPhotoService;
            _eventProcessor = eventProcessor;
        }

        public async Task HandleAsync(UpdateShelterPhoto command)
        {
            Core.Entities.Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);

            try
            {
                if (shelter.PhotoId != Guid.Empty)
                {
                    await _grpcPhotoService.DeleteAsync(shelter.PhotoId, BucketName.ShelterPhotos);
                }

                await _grpcPhotoService.AddAsync(command.Photo.Id, command.Photo.Name, command.Photo.Content,
                    BucketName.ShelterPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            shelter.UpdatePhoto(command.Photo.Id);

            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}