using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class UpdateShelterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public AddressModel Address { get; set; }
        [Required]
        public LocationModel GeoLocation { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}