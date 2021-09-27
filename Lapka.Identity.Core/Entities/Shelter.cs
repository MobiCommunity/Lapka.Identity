using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Core.Events.Concrete.Shelters;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        private ISet<Guid> _owners = new HashSet<Guid>();
        public string Name { get; private set; }
        public Address Address { get; }
        public Location GeoLocation { get; }
        public string PhotoPath { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public EmailAddress Email { get; private set; }
        public BankNumber BankNumber { get; private set; }
        public bool IsDeleted { get; private set; }

        public IEnumerable<Guid> Owners
        {
            get => _owners;
            private set => _owners = new HashSet<Guid>(value);
        }

        public Shelter(Guid id, string name, Address address, Location location, PhoneNumber phoneNumber,
            EmailAddress email, BankNumber bankNumber, string photoPath = null, bool isDeleted = false,
            IEnumerable<Guid> owners = null)
        {
            ValidShelter(name, phoneNumber);

            Id = new AggregateId(id);
            Name = name;
            Address = address;
            GeoLocation = location;
            PhotoPath = photoPath;
            PhoneNumber = phoneNumber;
            Email = email;
            BankNumber = bankNumber;
            Owners = owners ?? Enumerable.Empty<Guid>();
            IsDeleted = isDeleted;
        }

        public static Shelter Create(Guid id, string name, Address address, Location location, PhoneNumber phoneNumber,
            EmailAddress email, BankNumber bankNumber, string photoPath = null, IEnumerable<Guid> owners = null)
        {
            if (phoneNumber.IsEmpty)
            {
                
            }
            
            Shelter shelter = new Shelter(id, name, address, location, phoneNumber, email, bankNumber, photoPath, false,
                owners);

            shelter.AddEvent(new ShelterCreated(shelter));
            return shelter;
        }

        public void Delete()
        {
            IsDeleted = true;

            AddEvent(new ShelterDeleted(this));
        }

        public void Update(string name, PhoneNumber phoneNumber, EmailAddress email, BankNumber bankNumber)
        {
            ValidShelter(name, phoneNumber);

            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            BankNumber = bankNumber;

            AddEvent(new ShelterUpdated(this));
        }

        public void UpdatePhoto(string photoPath, string oldPhotoPath)
        {
            PhotoPath = photoPath;

            AddEvent(new ShelterPhotoUpdated(this, oldPhotoPath));
        }

        public void AddOwner(Guid ownerId)
        {
            _owners.Add(ownerId);

            AddEvent(new ShelterOwnerAdded(this, ownerId));
        }

        public void RemoveOwner(Guid ownerId)
        {
            _owners.Remove(ownerId);

            AddEvent(new ShelterOwnerRemoved(this, ownerId));
        }

        private static void ValidShelter(string name, PhoneNumber phoneNumber)
        {
            if (IsNameInvalid(name))
                throw new InvalidShelterNameException(name);

            if (IsPhoneNumberInvalid(phoneNumber))
                throw new InvalidPhoneNumberException(phoneNumber?.Value);
        }

        private static bool IsPhoneNumberInvalid(PhoneNumber phoneNumber) =>
            string.IsNullOrWhiteSpace(phoneNumber.Value);

        private static bool IsNameInvalid(string name) => string.IsNullOrWhiteSpace(name);
    }
}