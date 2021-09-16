using System;

namespace Lapka.Identity.Application.Dto.Shelters
{
    public class ShelterBasicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public Guid PhotoId { get; set; }
    }
}