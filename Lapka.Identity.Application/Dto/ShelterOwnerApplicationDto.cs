using System;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public class ShelterOwnerApplicationDto
    {
        public Guid Id { get; set; }
        public ShelterBasicDto Shelter { get; set; }
        public UserDto User { get; set; }
        public OwnerApplicationStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}