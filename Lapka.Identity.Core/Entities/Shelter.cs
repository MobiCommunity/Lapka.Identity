using System;
using System.Text.RegularExpressions;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        private const int BankNumberMinimumLetters = 5;
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public Location GeoLocation { get; private set; }
        public Guid PhotoId { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string BankNumber { get; private set; }

        public Shelter(Guid id, string name, Address address, Location location, Guid photoId, string phoneNumber,
            string email, string bankNumber)
        {
            ValidShelter(name, phoneNumber, email, bankNumber);

            Id = new AggregateId(id);
            Name = name;
            Address = address;
            GeoLocation = location;
            PhotoId = photoId;
            PhoneNumber = phoneNumber;
            Email = email;
            BankNumber = bankNumber;
        }

        public void Delete()
        {
            AddEvent(new ShelterDeleted(this));
        }
        
        public void Update(string name, Address address, Location location, string phoneNumber, string email, string bankNumber)
        {
            ValidShelter(name, phoneNumber, email, bankNumber);

            Name = name;
            Address = address;
            GeoLocation = location;
            PhoneNumber = phoneNumber;
            Email = email;
            BankNumber = bankNumber;
            
            AddEvent(new ShelterUpdated(this));
        }
        
        public void UpdatePhoto(Guid photoId)
        {
            PhotoId = photoId;
            
            AddEvent(new ShelterPhotoUpdated(this));
        }
        
        public static Shelter Create(Guid id, string name, Address address, Location location, Guid photoId,
            string phoneNumber, string email, string bankNumber)
        {
            Shelter shelter = new Shelter(id, name, address, location, photoId, phoneNumber, email, bankNumber);
            shelter.AddEvent(new ShelterCreated(shelter));
            return shelter;
        }

        private static void ValidShelter(string name, string phoneNumber, string email, string bankNumber)
        {
            if (IsNameInvalid(name))
                throw new InvalidShelterNameException(name);

            if (IsPhoneNumberInvalid(phoneNumber))
                throw new InvalidPhoneNumberException(phoneNumber);

            if (IsEmailInvalid(email))
                throw new InvalidEmailValueException(email);

            if (IsBankNumberInvalid(bankNumber))
                throw new InvalidBankNumberException(bankNumber);
        }

        private static bool IsEmailInvalid(string email) => string.IsNullOrWhiteSpace(email);

        private static bool IsPhoneNumberInvalid(string phoneNumber)
        {
            if (PhoneNumberRegex.IsMatch(phoneNumber))
            {
                return false;
            }

            return true;
        }
        private static bool IsNameInvalid(string name) => string.IsNullOrWhiteSpace(name);

        private static bool IsBankNumberInvalid(string bankNumber)
        {
            if (!string.IsNullOrEmpty(bankNumber))
            {
                return bankNumber.Length < BankNumberMinimumLetters;
            }

            return false;
        }
        
        private static readonly Regex PhoneNumberRegex =
            new Regex(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
    }
}