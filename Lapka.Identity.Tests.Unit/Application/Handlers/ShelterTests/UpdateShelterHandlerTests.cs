using System.Threading.Tasks;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.Handlers.Shelters;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;
using Lapka.Identity.Core.Entities;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.ShelterTests
{
    public class UpdateShelterHandlerTests
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly UpdateShelterHandler _handler;
        private readonly IShelterRepository _shelterRepository;

        public UpdateShelterHandlerTests()
        {
            _shelterRepository = Substitute.For<IShelterRepository>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new UpdateShelterHandler(_shelterRepository, _eventProcessor);
        }

        private Task Act(UpdateShelter command)
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

            UpdateShelter command = new UpdateShelter(shelter.Id.Value, userAuth, "new name", "111222333",
                "newemail@laapka.com", Extensions.ArrangeAddress("rzeszowska 101", "33-333 Rzeszów",
                    "Nowe City"), Extensions.ArrangeLocation(latitude: "5", longitude: "50"), "32483283284238");

            _shelterRepository.GetByIdAsync(command.Id).Returns(shelter);

            await Act(command);

            await _shelterRepository.Received().UpdateAsync(shelter);
            await _eventProcessor.Received().ProcessAsync(shelter.Events);
        }
    }
}