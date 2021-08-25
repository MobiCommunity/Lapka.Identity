using System;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Core.Entities.ShelterTests
{
    public class UpdateShelterTests
    {
        [Fact]
        public void given_valid_pet_should_be_updated()
        {
            Shelter shelter = ArrangeShelter();
            Shelter shelterUpdated = ArrangeShelter(name: "new name", address: ArrangeAddress(city: "new city"),
                location: ArrangeLocation(latitude: "5"), email: "newemail@lappka.com", phoneNumber: "tak");

            shelter.Update(shelterUpdated.Name, shelterUpdated.Address, shelterUpdated.GeoLocation,
                shelterUpdated.PhoneNumber, shelterUpdated.Email);

            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(shelter.Id);
            shelter.Name.ShouldBe(shelterUpdated.Name);
            shelter.Address.ShouldBe(shelterUpdated.Address);
            shelter.GeoLocation.ShouldBe(shelterUpdated.GeoLocation);
            shelter.PhoneNumber.ShouldBe(shelterUpdated.PhoneNumber);
            shelter.Email.ShouldBe(shelterUpdated.Email);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterUpdated>();
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