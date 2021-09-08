#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request.Auth
{
    public class SignUpRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName{ get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}