using System;
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
        
        public static BankNumber AsValueObject(this BankNumberModel model)
            => new BankNumber(model.Value);
        
        public static PhoneNumber AsValueObject(this ShelterPhoneNumberModel model)
            => new PhoneNumber(model.Value);
        
        public static PhoneNumber AsValueObject(this UserPhoneNumberModel model)
            => new PhoneNumber(model.Value);
        
        public static EmailAddress AsValueObject(this EmailAddressModel model)
            => new EmailAddress(model.Value);
        
        public static File AsValueObject(this IFormFile file) =>
            new File(file.FileName, file.OpenReadStream(), file.ContentType);
    }
}