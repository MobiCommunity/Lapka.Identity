using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class SignInGoogleRequest
    {
        [Required]
        public string AccessToken { get; set; }
    }
}