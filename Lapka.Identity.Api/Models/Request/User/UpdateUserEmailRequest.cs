using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class UpdateUserEmailRequest
    {
        [Required]
        public string Email { get; set; }
    }
}