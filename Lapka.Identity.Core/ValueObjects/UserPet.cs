using System;

namespace Lapka.Identity.Core.ValueObjects
{
    public class UserPet
    {
        public Guid PetId { get; set; }
        public Guid UserId { get; set; }
    }
}