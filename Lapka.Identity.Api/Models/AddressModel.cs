using System.ComponentModel.DataAnnotations;
using Lapka.Identity.Api.Models.Request;

namespace Lapka.Identity.Api.Models
{
    public class AddressModel
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
    }
}