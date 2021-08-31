using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Api.Models.Request
{
    public class UpdateShelterPhotoRequest
    {
        [Required]
        public IFormFile Photo { get; set; }
    }
}