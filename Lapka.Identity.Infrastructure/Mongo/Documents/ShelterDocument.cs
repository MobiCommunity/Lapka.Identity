using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class ShelterDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDocument Address { get; set; }
        public LocationDocument GeoLocation { get; set; }
        public Guid PhotoId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankNumber { get; set; }
        public List<Guid> Owners { get; set; }
        
    }
}