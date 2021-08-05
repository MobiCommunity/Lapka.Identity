using System;
using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public Location GeoLocation { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }

        public Shelter(Guid id, string name, Address address, Location location, string phoneNumber, string email)
        {
            Id = new AggregateId(id);
            Name = name;
            Address = address;
            GeoLocation = location;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public void Delete()
        {
            AddEvent(new ShelterDeleted(this));
        }
        
        public void Update(string name, Address address, Location location, string phoneNumber, string email)
        {
            Name = name;
            Address = address;
            GeoLocation = location;
            PhoneNumber = phoneNumber;
            Email = email;
            
            AddEvent(new ShelterUpdated(this));
        }
        
        public static Shelter Create(Guid id, string name, Address address, Location location, string phoneNumber, string email)
        {
            if (IsNameValid(name))
                throw new InvalidShelterNameException(name);

            if (IsPhoneNumberValid(phoneNumber))
                throw new InvalidPhoneNumberException(phoneNumber);

            if (IsEmailValid(email))
                throw new InvalidEmailValueException(email);
            
            Shelter shelter = new Shelter(id, name, address, location, phoneNumber, email);
            shelter.AddEvent(new ShelterCreated(shelter));
            return shelter;
        }

        private static bool IsEmailValid(string email) => string.IsNullOrWhiteSpace(email);

        private static bool IsPhoneNumberValid(string phoneNumber) => string.IsNullOrWhiteSpace(phoneNumber);


        private static bool IsNameValid(string name) => string.IsNullOrWhiteSpace(name);
    }
}