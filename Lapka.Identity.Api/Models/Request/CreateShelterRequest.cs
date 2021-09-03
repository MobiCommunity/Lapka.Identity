using System;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Api.Models.Request
{
    public class CreateShelterRequest
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
        [Required]
        public IFormFile Photo { get; set; }
        public string BankNumber { get; set; }
    }
}