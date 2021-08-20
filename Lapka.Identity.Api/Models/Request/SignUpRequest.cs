using System;

namespace Lapka.Identity.Api.Models.Request
{
    public class SignUpRequest
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoPath { get; set; }
        public string Role { get; set; }
    }
}