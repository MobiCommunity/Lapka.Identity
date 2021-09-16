using System;
using Convey.Types;

namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class JsonWebTokenDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}