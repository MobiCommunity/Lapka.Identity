using GeoCoordinatePortable;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Dto
{
    public static class Extensions
    {
        public static ShelterDto AsDto(this Shelter shelter, string latitude, string longitude)
        {
            double? distance = null;
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                Location location = new Location(latitude, longitude);
                GeoCoordinate pin1 = new GeoCoordinate(shelter.GeoLocation.Latitude.AsDouble(),
                    shelter.GeoLocation.Longitude.AsDouble());
                GeoCoordinate pin2 = new GeoCoordinate(location.Latitude.AsDouble(), location.Longitude.AsDouble());
                distance = pin1.GetDistanceTo(pin2);
            }

            return new ShelterDto
            {
                Id = shelter.Id.Value,
                Address = shelter.Address.AsDto(),
                Email = shelter.Email,
                GeoLocation = shelter.GeoLocation.AsDto(),
                PhotoId = shelter.PhotoId,
                Name = shelter.Name,
                PhoneNumber = shelter.PhoneNumber,
                Distance = distance
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
                Latitude = location.Latitude.AsDouble(),
                Longitude = location.Longitude.AsDouble()
            };
        }

        public static UserDto AsDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id.Value,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PhotoId = user.PhotoId,
                Role = user.Role,
                Username = user.Username,
            };
        }

        public static string GetFileExtension(this File file) =>
            file.Name.Contains('.') ? file.Name.Split('.')[1] : string.Empty;
    }
}