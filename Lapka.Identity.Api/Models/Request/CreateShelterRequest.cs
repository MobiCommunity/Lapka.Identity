using System;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Api.Models.Request
{
    public class CreateShelterRequest
    {
        public string Name { get; set; }
        public AddressModel Address { get; set; }
        public LocationModel GeoLocation { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}