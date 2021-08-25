using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.ShelterTests
{
    public class CreateShelterTests
    {
        private Shelter Act(AggregateId id, string name, Address address, Location location, string photoPath,
            string phoneNumber, string email) =>
            Shelter.Create(id.Value, name, address, location, photoPath, phoneNumber, email);
        
        [Fact]
        public void given_valid_shelter_should_be_created()
        {
            Shelter arrangeShelter = ArrangeShelter();
            
            Shelter shelter = Act(arrangeShelter.Id, arrangeShelter.Name, arrangeShelter.Address,
                arrangeShelter.GeoLocation, arrangeShelter.PhotoPath, arrangeShelter.PhoneNumber, arrangeShelter.Email);
            
            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(arrangeShelter.Id);
            shelter.Name.ShouldBe(arrangeShelter.Name);
            shelter.Address.ShouldBe(arrangeShelter.Address);
            shelter.GeoLocation.ShouldBe(arrangeShelter.GeoLocation);
            shelter.PhotoPath.ShouldBe(arrangeShelter.PhotoPath);
            shelter.PhoneNumber.ShouldBe(arrangeShelter.PhoneNumber);
            shelter.Email.ShouldBe(arrangeShelter.Email);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterCreated>();
        }
        
        [Fact]
        public void given_invalid_pet_name_should_throw_an_exception()
        {
            Shelter shelter = ArrangeShelter(name: "");
            
            Exception exception = Record.Exception(() => Act(shelter.Id, shelter.Name, shelter.Address,
                shelter.GeoLocation, shelter.PhotoPath, shelter.PhoneNumber, shelter.Email));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterNameException>();
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
        
        private Address ArrangeAddress(string name = null, string zipcode = null, string city = null)
        {
            string adressStreet = name ?? "Wojskowa";
            string addressZipcode = zipcode ?? "31-315 Rzeszów";
            string AddressCity = city ?? "Rzeszow";

            Address address = new Address(adressStreet, addressZipcode, AddressCity);

            return address;
        }

        private Location ArrangeLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude= latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";
            
            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }
    }
}