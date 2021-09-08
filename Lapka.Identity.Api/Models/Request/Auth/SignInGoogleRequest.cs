using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request.Auth
{
    public class SignInGoogleRequest
    {
        [Required]
        public string AccessToken { get; set; }
    }
}