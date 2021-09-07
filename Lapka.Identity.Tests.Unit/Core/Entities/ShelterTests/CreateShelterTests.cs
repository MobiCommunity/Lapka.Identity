using System;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Location;
using Lapka.Identity.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.ShelterTests
{
    public class CreateShelterTests
    {
        private Shelter Act(AggregateId id, string name, Address address, Location location, Guid photoPath,
            string phoneNumber, string email, string bankNumber) =>
            Shelter.Create(id.Value, name, address, location, photoPath, phoneNumber, email, bankNumber);

        [Theory]
        [InlineData("Złote łuki", "Aleji 25", "33-123 Rzeszów", "Rzeszów", "33,53253252", "55,623623535", "123123123",
            "schronisko@dog.com", "12123412341234123412341234")]
        [InlineData("Złote łuki", "Aleji 25", "33-123 Rzeszów", "Rzeszów", "33,53253252", "55,623623535", "123123123",
            "schronisko@dog.com", null)]
        [InlineData("Zł", "Al 25", "33-13 Rz", "Rz", "54,53253252", "5,623623535", "123123123",
            "sc@do.com", null)]
        public void given_valid_shelter_should_be_created(string name, string street, string zipCode, string city,
            string latitude, string longitude, string phoneNumber, string email, string bankNumber)
        {
            Shelter arrangeShelter = Extensions.ArrangeShelter();

            Shelter shelter = Act(arrangeShelter.Id, name, Extensions.ArrangeAddress(street, zipCode, city),
                Extensions.ArrangeLocation(latitude, longitude), arrangeShelter.PhotoId, phoneNumber, email, bankNumber);

            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(arrangeShelter.Id);
            shelter.Name.ShouldBe(name);
            shelter.Address.Street.ShouldBe(street);
            shelter.Address.ZipCode.ShouldBe(zipCode);
            shelter.Address.City.ShouldBe(city);
            shelter.GeoLocation.Latitude.Value.ShouldBe(latitude);
            shelter.GeoLocation.Longitude.Value.ShouldBe(longitude);
            shelter.PhotoId.ShouldBe(arrangeShelter.PhotoId);
            shelter.PhoneNumber.ShouldBe(phoneNumber);
            shelter.Email.ShouldBe(email);
            shelter.BankNumber.ShouldBe(bankNumber);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterCreated>();
        }

        [Fact]
        public void given_invalid_shelter_name_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(name: ""));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterNameException>();
        }
        
        [Fact]
        public void given_invalid_shelter_phone_number_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(phoneNumber: ""));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPhoneNumberException>();
        }

        [Fact]
        public void given_invalid_shelter_email_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(email: ""));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidEmailValueException>();
        }

        [Fact]
        public void given_invalid_shelter_address_city_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(address: Extensions.ArrangeAddress(city: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCityValueException>();
        }

        [Fact]
        public void given_invalid_shelter_address_name_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(address: Extensions.ArrangeAddress(street: "")));


            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStreetValueException>();
        }

        [Fact]
        public void given_invalid_shelter_address_zipcode_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(address: Extensions.ArrangeAddress(zipcode: "")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidZipCodeValueException>();
        }

        [Fact]
        public void given_invalid_shelter_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeLocation(latitude: ""));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLatitudeValueException>();
        }

        [Fact]
        public void given_too_big_shelter_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeLocation(latitude: "90"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_location_latitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeLocation(latitude: "-90"));


            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooLowException>();
        }

        [Fact]
        public void given_invalid_shelter_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeLocation(longitude: ""));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLongitudeValueException>();
        }

        [Fact]
        public void given_too_big_shelter_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeLocation(longitude: "180"));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_location_longitude_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeLocation(longitude: "-180"));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooLowException>();
        }
        
        [Fact]
        public void given_invalid_bank_number_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(bankNumber: "1234"));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBankNumberException>();
        }
    }
}