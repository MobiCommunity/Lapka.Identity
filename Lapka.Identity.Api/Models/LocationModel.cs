using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Api.Models.Request;

namespace Lapka.Identity.Api.Models
{
    public class LocationModel
    {
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
    }
}