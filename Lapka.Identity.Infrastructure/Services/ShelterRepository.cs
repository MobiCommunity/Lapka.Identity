using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Infrastructure.Services
{
    public class ShelterRepository : IShelterRepository
    {
        private readonly IList<Shelter> _shelters;
            
        public ShelterRepository()
        {
            _shelters = new List<Shelter>();
        }

        public Task AddAsync(Shelter shelter)
        {
            _shelters.Add(shelter);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Shelter shelter)
        {
            Shelter shelterFromDb = _shelters.FirstOrDefault(x => x.Id.Value == shelter.Id.Value);
            _shelters.Remove(shelterFromDb);

            return Task.CompletedTask;        
        }

        public Task UpdateAsync(Shelter shelter)
        {
            Shelter shelterFromDb = _shelters.FirstOrDefault(x => x.Id.Value == shelter.Id.Value);
            Shelter changedShelter = new Shelter(shelter.Id.Value, shelter.Name, shelter.Address, shelter.GeoLocation,
                shelter.PhoneNumber, shelter.Email);
            
            _shelters.Remove(shelterFromDb);
            _shelters.Add(changedShelter);
            
            return Task.CompletedTask;     
        }

        public Task<Shelter> GetByIdAsync(Guid id)
        {
            Shelter shelter = _shelters.FirstOrDefault(x => x.Id.Value == id);

            return Task.FromResult(shelter);
        } 
    }
}