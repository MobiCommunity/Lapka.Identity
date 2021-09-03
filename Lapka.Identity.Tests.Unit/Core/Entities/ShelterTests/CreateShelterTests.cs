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
        private Shelter Act(AggregateId id, string name, Address address, Location location, Guid photoPath,
            string phoneNumber, string email, string bankNumber) =>
            Shelter.Create(id.Value, name, address, location, photoPath, phoneNumber, email, bankNumber);
        
        [Fact]
        public void given_valid_shelter_should_be_created()
        {
            Shelter arrangeShelter = Extensions.ArrangeShelter();
            
            Shelter shelter = Act(arrangeShelter.Id, arrangeShelter.Name, arrangeShelter.Address,
                arrangeShelter.GeoLocation, arrangeShelter.PhotoId, arrangeShelter.PhoneNumber, arrangeShelter.Email, arrangeShelter.BankNumber);
            
            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(arrangeShelter.Id);
            shelter.Name.ShouldBe(arrangeShelter.Name);
            shelter.Address.ShouldBe(arrangeShelter.Address);
            shelter.GeoLocation.ShouldBe(arrangeShelter.GeoLocation);
            shelter.PhotoId.ShouldBe(arrangeShelter.PhotoId);
            shelter.PhoneNumber.ShouldBe(arrangeShelter.PhoneNumber);
            shelter.Email.ShouldBe(arrangeShelter.Email);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterCreated>();
        }
        
        [Fact]
        public void given_invalid_pet_name_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter(name: "");
            
            Exception exception = Record.Exception(() => Act(shelter.Id, shelter.Name, shelter.Address,
                shelter.GeoLocation, shelter.PhotoId, shelter.PhoneNumber, shelter.Email, shelter.BankNumber));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterNameException>();
        }
    }
}