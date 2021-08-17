using System;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public class ShelterDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public AddressDto Address { get; set; }
        [Required]
        public LocationDto GeoLocation { get; set; }
        [Required]
        public string PhotoPath { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
    }
}