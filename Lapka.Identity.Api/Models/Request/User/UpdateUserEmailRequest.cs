using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request.User
{
    public class UpdateUserEmailRequest
    {
        [Required]
        public string Email { get; set; }
    }
}