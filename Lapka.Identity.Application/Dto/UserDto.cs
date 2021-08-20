using System;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Dto
{
    public class UserDto
    {
        public Guid Id { get; }
        public string Username { get; }
        public string FirstName { get; }
        public string LastName{ get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string PhotoPath { get; }
        public DateTime CreatedAt { get; }
        public string Role { get; }

        public UserDto(User user)
        {
            Id = user.Id.Value;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            PhotoPath = user.PhotoPath;
            CreatedAt = user.CreatedAt;
            Role = user.Role;
        }
    }
}