using System.Threading.Tasks;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.Handlers.Shelters;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.ShelterTests
{
    public class DeleteShelterHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<DeleteShelterHandler> _logger;
        private readonly DeleteShelterHandler _handler;
        private readonly IGrpcPhotoService _photoService;
        private readonly IShelterRepository _shelterRepository;

        public DeleteShelterHandlerTests()
        {
            _shelterRepository = Substitute.For<IShelterRepository>();
            _logger = Substitute.For<ILogger<DeleteShelterHandler>>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new DeleteShelterHandler(_logger, _shelterRepository, _eventProcessor, _photoService);
        }

        private Task Act(DeleteShelter command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_shelter_should_update()
        {
            Shelter shelterArrange = Extensions.ArrangeShelter();
            UserAuth userAuth = Extensions.ArrangeUserAuth();

            Shelter shelter = Shelter.Create(shelterArrange.Id.Value, shelterArrange.Name, shelterArrange.Address,
                shelterArrange.GeoLocation, shelterArrange.PhotoId, shelterArrange.PhoneNumber, shelterArrange.Email,
                shelterArrange.BankNumber, shelterArrange.Owners);

            DeleteShelter command = new DeleteShelter(shelter.Id.Value, userAuth);

            _shelterRepository.GetByIdAsync(command.Id).Returns(shelter);

            await Act(command);

            await _shelterRepository.Received().DeleteAsync(shelter);
            await _photoService.DeleteAsync(shelter.PhotoId, BucketName.PetPhotos);
            await _eventProcessor.Received().ProcessAsync(shelter.Events);
        }
    }
}