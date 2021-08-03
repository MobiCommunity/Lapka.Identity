using System;
using Lapka.Identity.Core.Events.Concrete;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Core.Entities
{
    public class Shelter : AggregateRoot
    {
        public string Name { get; }
        public Address Address { get; }
        public Location GeoLocation { get; }
        public string PhoneNumber { get; }
        public string Email { get; }

        public Shelter(Guid id, string name, Address address, Location location, string phoneNumber, string email)
        {
            Id = new AggregateId(id);
            Name = name;
            Address = address;
            GeoLocation = location;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        
        public static Shelter Create(Guid id, string name, Address address, Location location, string phoneNumber, string email)
        {

            
            Shelter shelter = new Shelter(id, name, address, location, phoneNumber, email);
            shelter.AddEvent(new ShelterCreated(shelter));
            return shelter;
        }
    }
    
}