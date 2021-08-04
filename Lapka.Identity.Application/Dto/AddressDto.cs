using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Application.Dto
{
    public class AddressDto
    {
        [Required]
        public string Street { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
    }
}