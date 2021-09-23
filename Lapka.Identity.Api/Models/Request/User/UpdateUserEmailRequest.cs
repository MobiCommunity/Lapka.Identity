using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request.User
{
    public class UpdateUserEmailRequest
    {
        [Required]
        public EmailAddressModel Email { get; set; }
    }
}