using Lapka.Identity.Core.Exceptions;

namespace Lapka.Identity.Core.ValueObjects
{
    public class Address
    {
        public string Street { get; }
        public string ZipCode { get; }
        public string City { get; }

        public Address(string street, string zipCode, string city)
        {
            Street = street;
            ZipCode = zipCode;
            City = city;
            
            Validate();
        }

        private void Validate()
        {
            ValidateStreet(Street);
            ValidateZipCode(Street);
            ValidateCity(Street);
        }

        private static void ValidateStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new InvalidStreetValueException(street);
            }
        }
        private static void ValidateZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                throw new InvalidZipCodeValueException(zipCode);
            }
        }
        private static void ValidateCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new InvalidCityValueException(city);
            }
        }
    }
}