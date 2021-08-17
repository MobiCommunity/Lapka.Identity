using System;
using Convey.Types;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Infrastructure.Documents
{
    public class ShelterDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDocument Address { get; set; }
        public LocationDocument GeoLocation { get; set; }
        public string PhotoPath { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}