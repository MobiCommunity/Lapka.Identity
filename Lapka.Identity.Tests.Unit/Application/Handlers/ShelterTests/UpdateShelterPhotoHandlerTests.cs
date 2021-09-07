using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers.Shelter;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.ShelterTests
{
    public class UpdateShelterPhotoHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly UpdateShelterPhotoHandler _handler;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;

        public UpdateShelterPhotoHandlerTests()
        {
            _shelterRepository = Substitute.For<IShelterRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdateShelterPhotoHandler(_shelterRepository, _photoService, _eventProcessor);
        }

        private Task Act(UpdateShelterPhoto command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_shelter_should_update()
        {
            Guid photoId = Guid.NewGuid();
            Shelter shelterArrange = Extensions.ArrangeShelter();
            Guid oldPhotoId = shelterArrange.PhotoId;
            PhotoFile file = Extensions.ArrangePhotoFile(photoId);

            Shelter shelter = Shelter.Create(shelterArrange.Id.Value, shelterArrange.Name, shelterArrange.Address,
                shelterArrange.GeoLocation, shelterArrange.PhotoId, shelterArrange.PhoneNumber, shelterArrange.Email, shelterArrange.BankNumber);

            UpdateShelterPhoto command = new UpdateShelterPhoto(shelter.Id.Value, file);

            _shelterRepository.GetByIdAsync(command.Id).Returns(shelter);

            await Act(command);

            await _shelterRepository.Received().UpdateAsync(shelter);
            await _photoService.DeleteAsync(oldPhotoId, BucketName.PetPhotos);
            await _photoService.AddAsync(file.Id, file.Name, file.Content, BucketName.PetPhotos);
            await _eventProcessor.Received().ProcessAsync(shelter.Events);
        }
    }
}