using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Api.Models.Request.User
{
    public class UpdateUserPhotoRequest
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}