using Lapka.Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Api.Models
{
    public static class Extensions
    {
        public static Address AsValueObject(this AddressModel model) 
            => new Address(model.Street, model.ZipCode, model.City);

        public static Location AsValueObject(this LocationModel model)
            => new Location(model.Latitude, model.Longitude);
        
        public static File AsValueObject(this IFormFile file) =>
            new File(file.FileName, file.OpenReadStream(), file.ContentType);
    }
}