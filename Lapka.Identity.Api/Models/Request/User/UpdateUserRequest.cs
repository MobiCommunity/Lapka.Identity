using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request.User
{
    public class UpdateUserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName{ get; set; }
        public UserPhoneNumberModel PhoneNumber { get; set; }
    }
}