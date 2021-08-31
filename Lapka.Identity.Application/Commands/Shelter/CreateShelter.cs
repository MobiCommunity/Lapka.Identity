using System;
using System.Text;
using Convey.CQRS.Commands;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands
{
    public class CreateShelter : ICommand
    {
        public Guid Id { get; }
        public string Name { get; }
        public Location GeoLocation { get; }
        public Address Address { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public PhotoFile Photo { get; }

        public CreateShelter(Guid id, string name, string phoneNumber, string email, Address address, 
            Location geoLocation, PhotoFile photo)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            GeoLocation = geoLocation;
            Photo = photo;
        }
    }
}