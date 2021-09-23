using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }
    }
}