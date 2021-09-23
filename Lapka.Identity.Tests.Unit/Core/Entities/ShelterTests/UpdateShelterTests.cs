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
        [InlineData("Złote łuki", "123123123", "schronisko@dog.com", "12123412341234123412341234")]
        [InlineData("Złote łuki", "123123123", "schronisko@dog.com", null)]
        [InlineData("Zł",  "123123123", "sc@do.com", null)]
        public void given_valid_pet_should_be_updated(string name, string phoneNumber, string email, string bankNumber)
        {
            Shelter shelter = Extensions.ArrangeShelter();

            shelter.Update(name, new PhoneNumber(phoneNumber), new EmailAddress(email), new BankNumber(bankNumber));

            shelter.ShouldNotBeNull();
            shelter.Id.ShouldBe(shelter.Id);
            shelter.Name.ShouldBe(name);
            shelter.PhotoPath.ShouldBe(shelter.PhotoPath);
            shelter.PhoneNumber.Value.ShouldBe(phoneNumber);
            shelter.Email.Value.ShouldBe(email);
            shelter.BankNumber.Value.ShouldBe(bankNumber);
            shelter.Events.Count().ShouldBe(1);
            IDomainEvent @event = shelter.Events.Single();
            @event.ShouldBeOfType<ShelterUpdated>();
        }

        [Fact]
        public void given_invalid_shelter_name_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() =>
                shelter.Update("", shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidShelterNameException>();
        }

        [Theory]
        [InlineData("12312312")]
        [InlineData("1231231231")]
        [InlineData("")]
        [InlineData(null)]
        public void given_invalid_shelter_phone_number_should_throw_an_exception(string phoneNumber)
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() =>
                shelter.Update(shelter.Name, new PhoneNumber(phoneNumber), shelter.Email, shelter.BankNumber));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidPhoneNumberException>();
        }

        [Fact]
        public void given_invalid_shelter_email_should_throw_an_exception()
        {
            Shelter shelter = Extensions.ArrangeShelter();
            Exception exception = Record.Exception(() =>
                shelter.Update(shelter.Name, shelter.PhoneNumber, new EmailAddress(""), shelter.BankNumber));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidEmailValueException>();
        }
        

        [Fact]
        public void given_invalid_bank_number_should_throw_an_exception()
        {
            Exception exception = Record.Exception(() => Extensions.ArrangeShelter(bankNumber: new BankNumber("1234")));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidBankNumberException>();
        }
    }
}