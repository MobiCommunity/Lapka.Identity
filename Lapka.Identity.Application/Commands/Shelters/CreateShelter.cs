using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Shelters
{
    public class CreateShelter : ICommand
    {
        public Guid Id { get; }
        public UserAuth UserAuth { get; }
        public string Name { get; }
        public Location GeoLocation { get; }
        public Address Address { get; }
        public PhoneNumber PhoneNumber { get; }
        public EmailAddress Email { get; }
        public File Photo { get; }
        public BankNumber BankNumber { get; }

        public CreateShelter(Guid id, UserAuth userAuth, string name, PhoneNumber phoneNumber, EmailAddress email,
            Address address, Location geoLocation, File photo, BankNumber bankNumber)
        {
            Id = id;
            UserAuth = userAuth;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            GeoLocation = geoLocation;
            Photo = photo;
            BankNumber = bankNumber;
        }
    }
}