using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Grpc;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers.Shelters
{
    public class UpdateShelterPhotoHandler : ICommandHandler<UpdateShelterPhoto>
    {
        private readonly ILogger<UpdateShelterPhotoHandler> _logger;
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IEventProcessor _eventProcessor;

        public UpdateShelterPhotoHandler(ILogger<UpdateShelterPhotoHandler> logger,
            IShelterRepository shelterRepository, IGrpcPhotoService grpcPhotoService, IEventProcessor eventProcessor)
        {
            _logger = logger;
            _shelterRepository = shelterRepository;
            _grpcPhotoService = grpcPhotoService;
            _eventProcessor = eventProcessor;
        }

        public async Task HandleAsync(UpdateShelterPhoto command)
        {
            Shelter shelter = await GetShelterAsync(command);

            ValidIfUserIsAccessibleToModifyPet(command, shelter);

            await UpdatePhotoInMinioAsync(command, shelter);
            shelter.UpdatePhoto(command.Photo.Id);

            await _shelterRepository.UpdateAsync(shelter);
            await _eventProcessor.ProcessAsync(shelter.Events);
        }

        private static void ValidIfUserIsAccessibleToModifyPet(UpdateShelterPhoto command, Shelter shelter)
        {
            if (shelter.Owners.Any(x => x != command.UserAuth.UserId) && command.UserAuth.Role != "admin")
            {
                throw new Exceptions.UnauthorizedAccessException();
            }
        }

        private async Task<Shelter> GetShelterAsync(UpdateShelterPhoto command)
        {
            Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);
            if (shelter == null)
            {
                throw new ShelterNotFoundException(command.Id.ToString());
            }

            return shelter;
        }

        private async Task UpdatePhotoInMinioAsync(UpdateShelterPhoto command, Shelter shelter)
        {
            await DeletePhotoFromMinioAsync(shelter);
            await AddPhotoFromMinioAsync(command);
        }

        private async Task AddPhotoFromMinioAsync(UpdateShelterPhoto command)
        {
            try
            {
                await _grpcPhotoService.AddAsync(command.Photo.Id, command.Photo.Name, command.Photo.Content,
                    BucketName.ShelterPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
        }

        private async Task DeletePhotoFromMinioAsync(Shelter shelter)
        {
            try
            {
                if (shelter.PhotoId != Guid.Empty)
                {
                    await _grpcPhotoService.DeleteAsync(shelter.PhotoId, BucketName.ShelterPhotos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete photo, but passing to add photo");
            }
        }
    }
}