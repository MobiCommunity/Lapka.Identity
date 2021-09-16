using System;
using System.Linq;
using System.Runtime.InteropServices;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Events.Concrete.Shelters;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.Exceptions.Location;
using Lapka.Identity.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.ShelterTests
{
    public class UpdateShelterTests
    {
        [Theory]
        [InlineData("Złote łuki", "Aleji 25", "33-123 Rzeszów", "Rzeszów", "33,53253252", "55,623623535", "123123123",
            "schronisko@dog.com", "12123412341234123412341234")]
        [InlineData("Złote łuki", "Aleji 25", "33-123 Rzeszów", "Rzeszów", "33,53253252", "55,623623535", "123123123",
            "schronisko@dog.com", null)]
        [InlineData("Zł", "Al 25", "33-13 Rz", "Rz", "54,53253252", "5,623623535", "123123123",
            "sc@do.com", null)]
        public void given_valid_pet_should_be_updated(string name, string street, string zipCode, string city,
            string latitude, string longitude, string phoneNumber, string email, string bankNumber)
        {
            Shelter shelter = Extensions.ArrangeShelter();

            shelter.Update(name, new Address(street, zipCode, city), new Location(latitude, longitude), phoneNumber,
                email, bankNumber);

            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(shelter.Id);
            shelter.Name.ShouldBe(name);
            shelter.Address.Street.ShouldBe(street);
            shelter.Address.ZipCode.ShouldBe(zipCode);
            shelter.Address.City.ShouldBe(city);
            shelter.GeoLocation.Latitude.Value.ShouldBe(latitude);
            shelter.GeoLocation.Longitude.Value.ShouldBe(longitude);
            shelter.PhotoId.ShouldBe(shelter.PhotoId);
            shelter.PhoneNumber.ShouldBe(phoneNumber);
            shelter.Email.ShouldBe(email);
            shelter.BankNumber.ShouldBe(bankNumber);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterUpdated>();
        }

        [Fact]
        public void given_invalid_shelter_name_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update("", shelter.Address, shelter.GeoLocation,
                shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterNameException>();
        }

        [Theory]
        [InlineData("12312312")]
        [InlineData("1231231231")]
        [InlineData("")]
        public void given_invalid_shelter_phone_number_should_throw_an_exception(string phoneNumber)
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                shelter.GeoLocation,
                phoneNumber, shelter.Email, shelter.BankNumber));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPhoneNumberException>();
        }

        [Fact]
        public void given_invalid_shelter_email_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                shelter.GeoLocation,
                shelter.PhoneNumber, "", shelter.BankNumber));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidEmailValueException>();
        }

        [Fact]
        public void given_invalid_shelter_address_city_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name,
                Extensions.ArrangeAddress(city: ""), shelter.GeoLocation,
                shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCityValueException>();
        }

        [Fact]
        public void given_invalid_shelter_address_name_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name,
                Extensions.ArrangeAddress(street: ""), shelter.GeoLocation,
                shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidStreetValueException>();
        }

        [Fact]
        public void given_invalid_shelter_address_zipcode_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name,
                Extensions.ArrangeAddress(zipcode: ""), shelter.GeoLocation,
                shelter.PhoneNumber, shelter.Email, shelter.BankNumber));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidZipCodeValueException>();
        }

        [Fact]
        public void given_invalid_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                Extensions.ArrangeLocation(latitude: ""), shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLatitudeValueException>();
        }

        [Fact]
        public void given_too_big_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                Extensions.ArrangeLocation(latitude: "90"), shelter.PhoneNumber, shelter.Email, shelter.BankNumber));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_location_latitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                Extensions.ArrangeLocation(latitude: "-90"), shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LatitudeTooLowException>();
        }

        [Fact]
        public void given_invalid_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                Extensions.ArrangeLocation(longitude: ""), shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidLongitudeValueException>();
        }

        [Fact]
        public void given_too_big_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                Extensions.ArrangeLocation(longitude: "180"), shelter.PhoneNumber, shelter.Email, shelter.BankNumber));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<LongitudeTooBigException>();
        }

        [Fact]
        public void given_too_low_shelter_location_longitude_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() => shelter.Update(shelter.Name, shelter.Address,
                Extensions.ArrangeLocation(longitude: "-180"), shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

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