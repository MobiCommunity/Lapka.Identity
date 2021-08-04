using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var shelterFromDb = _shelters.FirstOrDefault(x => x.Id.Value == shelter.Id.Value);
            _shelters.Remove(shelterFromDb);

            return Task.CompletedTask;        
        }

        public Task<Shelter> GetByIdAsync(Guid id)
        {
            var shelter = _shelters.FirstOrDefault(x => x.Id.Value == id);

            return Task.FromResult(shelter);
        }
    }
    
}