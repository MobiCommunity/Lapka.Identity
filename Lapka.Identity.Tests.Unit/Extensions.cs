using System;
using System.Collections.Generic;
using System.IO;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using File = Lapka.Identity.Core.ValueObjects.File;

namespace Lapka.Identity.Tests.Unit
{
    public static class Extensions
    {
        public static User ArrangeUser(AggregateId id = null, string username = null, string firstName = null,
            string lastName = null, string email = null, string password = null, DateTime createdAt = default,
            string role = null, List<Guid> userPets = null)
        {
            AggregateId validId = id ?? new AggregateId();
            string validUsername = username ?? "Pomidorowy";
            string validFirstName = firstName ?? "Jasiek";
            string validLastName = lastName ?? "Skronowski";
            string validEmail = email ?? "Skronowski@email.com";
            string validPassword = password ?? "Secretpassword";
            string validRole = role ?? "user";
            DateTime validCreatedAt = DateTime.UtcNow;
            if (createdAt != default)
            {
                validCreatedAt = createdAt;
            }

            User user = new User(validId.Value, validUsername, validFirstName, validLastName, validEmail, validPassword,
                null, Guid.Empty, validCreatedAt, validRole);

            return user;
        }

        public static Shelter ArrangeShelter(AggregateId id = null, string name = null, Address address = null,
            Location location = null, Guid? photoId = null, string phoneNumber = null, string email = null,
            string bankNumber = null, List<Guid> owners = null)
        {
            AggregateId validId = id ?? new AggregateId();
            string validName = name ?? "Miniok";
            Address validAddress = address ?? ArrangeAddress();
            Location validLocation = location ?? ArrangeLocation();
            Guid validPhotoId = photoId ?? Guid.NewGuid();
            string validPhoneNumber = phoneNumber ?? "435731934";
            string validEmail = email ?? "support@lappka.com";
            string validBankNumber = bankNumber ?? "24204530505030050350535035";
            List<Guid> validOwners = owners ?? new List<Guid>();

            Shelter shelter = new Shelter(validId.Value, validName, validAddress, validLocation, validPhotoId,
                validPhoneNumber, validEmail, validBankNumber, validOwners);

            return shelter;
        }

        public static Address ArrangeAddress(string street = null, string zipcode = null, string city = null)
        {
            string adressStreet = street ?? "Wojskowa";
            string addressZipcode = zipcode ?? "31-315 Rzeszów";
            string AddressCity = city ?? "Rzeszow";

            Address address = new Address(adressStreet, addressZipcode, AddressCity);

            return address;
        }

        public static Location ArrangeLocation(string latitude = null, string longitude = null)
        {
            string shelterAddressLocationLatitude = latitude ?? "22";
            string shelterAddressLocationLongitude = longitude ?? "33";

            Location location = new Location(shelterAddressLocationLatitude, shelterAddressLocationLongitude);

            return location;
        }

        public static PhotoFile ArrangePhotoFile(Guid? id = null, string name = null, Stream stream = null,
            string contentType = null)
        {
            Guid validId = id ?? Guid.NewGuid();
            string validName = name ?? $"{Guid.NewGuid()}.jpg";
            Stream validStream = stream ?? new MemoryStream();
            string validContentType = contentType ?? "image/jpg";

            PhotoFile file = new PhotoFile(validId, validName, validStream, validContentType);

            return file;
        }

        public static UserAuth ArrangeUserAuth(Guid? id = null, string role = null)
        {
            Guid validId = id ?? Guid.NewGuid();
            string validRole = role ?? $"admin";

            UserAuth userAuth = new UserAuth(validRole, validId);

            return userAuth;
        }
    }
}