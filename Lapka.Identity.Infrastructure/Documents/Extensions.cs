using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Documents;


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
                PhotoPath = shelter.PhotoPath,
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
                shelter.GeoLocation.AsBusiness(), shelter.PhotoPath, shelter.PhoneNumber, shelter.Email);
        }
        
        public static Location AsBusiness(this LocationDocument shelter)
        {
            return new Location(shelter.Latitude, shelter.Longitude);
        }

        public static Address AsBusiness(this AddressDocument shelter)
        {
            return new Address(shelter.Street, shelter.ZipCode, shelter.City);
        }
        public static UserDocument AsDocument(this User user)
        {
            return new UserDocument
            {
                Id = user.Id.Value,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                PhotoPath = user.PhotoPath,
                Role = user.Role
            };
        }
        
        public static User AsBusiness(this UserDocument user)
        {
            return new User(user.Id, user.Username, user.FirstName, user.LastName, user.Email, user.Password, 
                user.PhoneNumber, user.PhotoPath, user.CreatedAt, user.Role);
        }

        public static JsonWebToken AsBusiness(this Convey.Auth.JsonWebToken jsonWebToken)
        {
            return new JsonWebToken(jsonWebToken.AccessToken, jsonWebToken.RefreshToken, jsonWebToken.Expires);
        }

        public static RefreshToken AsBusiness(this JsonWebTokenDocument token)
        {
            return new RefreshToken(token.Id, token.UserId, token.RefreshToken, token.CreatedAt, token.ExpiresAt);
        }
        
        public static JsonWebTokenDocument AsDocument(this RefreshToken token)
        {
            return new JsonWebTokenDocument
            {
                Id = token.Id.Value,
                UserId = token.UserId,
                RefreshToken = token.Token,
                CreatedAt = token.CreatedAt,
                ExpiresAt = token.RevokedAt
            };
        }
    }
}