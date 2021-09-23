using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models
{
    public class ShelterPhoneNumberModel
    {
        [Required]
        public string Value { get; set; }
    }
}