using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public static class Extensions
    {
        public static ShelterDto AsDto(this Shelter shelter)
        {
            return new ShelterDto
            {
                Name = shelter.Name,
                Address = shelter.Address.AsDto(),
                Email = shelter.Email,
                GeoLocation = shelter.GeoLocation.AsDto(),
                PhoneNumber = shelter.PhoneNumber
            };
        }
        
        public static ValueDto AsDto(this Value value)
        {
            return new ValueDto
            {
                Name = value.Name,
                Description = value.Description,
                Id = value.Id.Value
            };
        }
        
        public static AddressDto AsDto(this Address address)
        {
            return new AddressDto
            {
                Street = address.Street,
                City = address.City,
                ZipCode = address.ZipCode
            };
        }
        
        public static LocationDto AsDto(this Location location)
        {
            return new LocationDto
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }
    }
}