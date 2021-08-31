using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class SignInFacebookRequest
    {
        [Required]
        public string AccessToken { get; set; }
    }
}