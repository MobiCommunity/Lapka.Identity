using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request.Auth
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}