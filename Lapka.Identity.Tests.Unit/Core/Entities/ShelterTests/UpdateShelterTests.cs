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
            Shelter shelter = Extensions.ArrangeShelter();
            Shelter shelterUpdated = Extensions.ArrangeShelter(name: "new name", address: Extensions.ArrangeAddress(city: "new city"),
                location: Extensions.ArrangeLocation(latitude: "5"), email: "newemail@lappka.com", phoneNumber: "tak", bankNumber: "3848328482");

            shelter.Update(shelterUpdated.Name, shelterUpdated.Address, shelterUpdated.GeoLocation,
                shelterUpdated.PhoneNumber, shelterUpdated.Email, shelterUpdated.BankNumber);

            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(shelter.Id);
            shelter.Name.ShouldBe(shelterUpdated.Name);
            shelter.Address.ShouldBe(shelterUpdated.Address);
            shelter.GeoLocation.ShouldBe(shelterUpdated.GeoLocation);
            shelter.PhoneNumber.ShouldBe(shelterUpdated.PhoneNumber);
            shelter.Email.ShouldBe(shelterUpdated.Email);
            shelter.BankNumber.ShouldBe(shelterUpdated.BankNumber);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterUpdated>();
        }
    }
}