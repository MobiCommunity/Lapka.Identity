using System;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.Handlers.Shelters;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
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
        private readonly ILogger<UpdateShelterPhotoHandler> _logger;

        public UpdateShelterPhotoHandlerTests()
        {
            _logger = Substitute.For<ILogger<UpdateShelterPhotoHandler>>();
            _shelterRepository = Substitute.For<IShelterRepository>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdateShelterPhotoHandler(_logger, _shelterRepository, _photoService, _eventProcessor);
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
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Shelter shelter = Shelter.Create(shelterArrange.Id.Value, shelterArrange.Name, shelterArrange.Address,
                shelterArrange.GeoLocation, shelterArrange.PhotoId, shelterArrange.PhoneNumber, shelterArrange.Email, shelterArrange.BankNumber, shelterArrange.Owners);

            UpdateShelterPhoto command = new UpdateShelterPhoto(shelter.Id.Value, userAuth, file);

            _shelterRepository.GetByIdAsync(command.Id).Returns(shelter);

            await Act(command);

            await _shelterRepository.Received().UpdateAsync(shelter);
            await _photoService.DeleteAsync(oldPhotoId, BucketName.PetPhotos);
            await _photoService.AddAsync(file.Id, file.Name, file.Content, BucketName.PetPhotos);
            await _eventProcessor.Received().ProcessAsync(shelter.Events);
        }
    }
}