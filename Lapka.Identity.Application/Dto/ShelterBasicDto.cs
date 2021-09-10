using System;
using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public class ShelterBasicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public Guid PhotoId { get; set; }
    }
}