using System;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands
{
    public class UpdateShelter : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public Location GeoLocation { get; }
        public Address Address { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public string? BankNumber { get; }


        public UpdateShelter(Guid id, string name, string phoneNumber, string email, Address address,
            Location geoLocation, string bankNumber)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            GeoLocation = geoLocation;
            BankNumber = bankNumber;
        }
    }
}