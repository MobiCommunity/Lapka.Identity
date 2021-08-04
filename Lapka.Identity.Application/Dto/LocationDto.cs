<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
=======
﻿using System.ComponentModel.DataAnnotations;
>>>>>>> origin/develop

namespace Lapka.Identity.Application.Dto
{
    public class LocationDto
    {
        [Required] 
        public string Latitude { get; set; }
        [Required] 
        public string Longitude { get; set; }
    }
        

}