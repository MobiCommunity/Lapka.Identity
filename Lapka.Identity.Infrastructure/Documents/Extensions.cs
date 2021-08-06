using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;


namespace Lapka.Identity.Infrastructure.Exceptions
{
    public static class Extensions
    {
        public static ShelterDocument AsDocument(this Shelter shelter)
        {
            return new ShelterDocument
            {
                Id = shelter.Id.Value,
                Name = shelter.Name,
                Address = shelter.Address.AsDocument(),
                GeoLocation = shelter.GeoLocation.AsDocument(),
                Email = shelter.Email,
                PhoneNumber = shelter.PhoneNumber
            };
        }

        public static LocationDocument AsDocument(this Location shelter)
        {
            return new LocationDocument
            {
                Latitude = shelter.Latitude,
                Longitude = shelter.Longitude
            };
        }
        
        public static AddressDocument AsDocument(this Address shelter)
        {
            return new AddressDocument
            {
                Street = shelter.Street,
                City = shelter.City,
                ZipCode = shelter.ZipCode
            };
        }
        
        public static Shelter AsBusiness(this ShelterDocument shelter)
        {
            return new Shelter(shelter.Id, shelter.Name, shelter.Address.AsBusiness(),
                shelter.GeoLocation.AsBusiness(), shelter.PhoneNumber, shelter.Email);
        }
        
        public static Location AsBusiness(this LocationDocument shelter)
        {
            return new Location(shelter.Latitude, shelter.Longitude);
        }
        
        public static Address AsBusiness(this AddressDocument shelter)
        {
            return new Address(shelter.Street, shelter.ZipCode, shelter.City);
        }
    }
}