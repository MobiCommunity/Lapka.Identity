using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Api.Models
{
    public static class Extensions
    {
        public static Address AsValueObject(this AddressModel model) 
            => new Address(model.Street, model.ZipCode, model.City);

        public static Location AsValueObject(this LocationModel model)
            => new Location(model.Latitude, model.Longitude);

    }
}