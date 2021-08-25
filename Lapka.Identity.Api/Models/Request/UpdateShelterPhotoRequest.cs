using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Api.Models.Request
{
    public class UpdateShelterPhotoRequest
    {
        public IFormFile Photo { get; set; }
    }
}