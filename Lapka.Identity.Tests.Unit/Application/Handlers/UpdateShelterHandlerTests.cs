using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using File = Lapka.Identity.Core.ValueObjects.File;

namespace Lapka.Identity.Tests.Unit.Application.Handlers
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
            Shelter shelterArrange = ArrangeShelter();

            Shelter shelter = Shelter.Create(shelterArrange.Id.Value, shelterArrange.Name, shelterArrange.Address,
                shelterArrange.GeoLocation, shelterArrange.PhotoPath, shelterArrange.PhoneNumber, shelterArrange.Email);

            UpdateShelter command = new UpdateShelter(shelter.Id.Value, "new name", "111222333",
                "newemail@laapka.com", ArrangeAddress("rzeszowska 101", "33-333 Rzeszów",
                    "Nowe City"), ArrangeLocation(latitude: "5", longitude: "50"));

            _shelterRepository.GetByIdAsync(command.Id).Returns(shelter);

            await Act(command);

            await _shelterRepository.Received().UpdateAsync(shelter);
            await _eventProcessor.Received().ProcessAsync(shelter.Events);
        }

        private Shelter ArrangeShelter(AggregateId id = null, string name = null, Address address = null,
            Location location = null, string photoPath = null, string phoneNumber = null, string email = null)
        {
            AggregateId validId = id ?? new AggregateId();
            string validName = name ?? "Miniok";
            Address validAddress = address ?? ArrangeAddress();
            Location validLocation = location ?? ArrangeLocation();
            string validPhotoPath = photoPath ?? $"{Guid.NewGuid()}.jpg";
            string validPhoneNumber = phoneNumber ?? "435731934";
            string validEmail = email ?? "support@lappka.com";

            Shelter shelter = new Shelter(validId.Value, validName, validAddress, validLocation, validPhotoPath,
                validPhoneNumber, validEmail);

            return shelter;
        }

        private Address ArrangeAddress(string street = null, string zipcode = null, string city = null)
        {
            string adressStreet = street ?? "Wojskowa";
            string addressZipcode = zipcode ?? "31-315 Rzeszów";
            string AddressCity = city ?? "Rzeszow";

            Address address = new Address(adressStreet, addressZipcode, AddressCity);

            return address;
        }

        private Location ArrangeLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }
    }
}