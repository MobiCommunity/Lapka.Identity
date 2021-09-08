using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request.Auth
{
    public class SignInFacebookRequest
    {
        [Required]
        public string AccessToken { get; set; }
    }
}