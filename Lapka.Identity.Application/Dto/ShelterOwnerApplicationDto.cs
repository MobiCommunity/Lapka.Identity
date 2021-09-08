using System;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public class ShelterOwnerApplicationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShelterId { get; set; }
        public OwnerApplicationStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}