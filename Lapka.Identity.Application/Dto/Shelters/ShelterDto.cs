using System;

namespace Lapka.Identity.Application.Dto.Shelters
{
    public class ShelterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public LocationDto GeoLocation { get; set; }
        public Guid PhotoId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double? Distance { get; set; }
        public string BankNumber { get; set; }
    }
}