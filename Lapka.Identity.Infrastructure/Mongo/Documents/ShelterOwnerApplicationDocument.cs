using System;
using Convey.Types;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class ShelterOwnerApplicationDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid ShelterId { get; set; }
        public Guid UserId { get; set; }
        public OwnerApplicationStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}