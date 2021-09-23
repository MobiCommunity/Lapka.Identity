using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models
{
    public class EmailAddressModel
    {
        [Required]
        public string Value { get; set; }
    }
}