using System;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Api.Models.Request
{
    public class CreateShelterRequest
    {
        public Guid Id { get; }
        public string Name { get; }
        public AddressModel Address { get; }
        public LocationModel GeoLocation { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
    }
}