using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}