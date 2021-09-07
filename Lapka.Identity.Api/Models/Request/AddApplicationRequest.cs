using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Identity.Api.Models.Request
{
    public class AddApplicationRequest
    {
        [Required]
        public Guid ShelterId { get; set; }
    }
}