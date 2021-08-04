using System;

namespace Lapka.Identity.Api.Models.Request
{
    public class UpdateShelterRequest
    {
        public string Name { get; set; }
        public AddressModel Address { get; set; }
        public LocationModel GeoLocation { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}