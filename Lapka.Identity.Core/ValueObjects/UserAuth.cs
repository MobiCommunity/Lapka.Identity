using System;

namespace Lapka.Identity.Api.Models
{
    public class UserAuth
    {
        public string Role { get; }
        public Guid UserId { get; }

        public UserAuth(string role, Guid userId)
        {
            Role = role;
            UserId = userId;
        }
    }
}