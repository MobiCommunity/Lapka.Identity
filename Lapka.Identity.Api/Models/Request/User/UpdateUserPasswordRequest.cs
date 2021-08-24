using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class UpdateUserPasswordRequest
    {
        [Required]
        public string Password { get; set; }
    }
}