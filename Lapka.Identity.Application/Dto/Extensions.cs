using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public static class Extensions
    {
        public static Address AsDto(this AddressDto addressDto)
            => new Address(addressDto.City, addressDto.Street, addressDto.ZipCode);

        public static Location AsDto(this LocationDto locationDto)
            => new Location(locationDto.Latitude, locationDto.Longitude);


        public static ShelterDto AsDto(this Shelter shelter)
        {
            return new ShelterDto
            {
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