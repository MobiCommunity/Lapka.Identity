using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class RevokeRefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}