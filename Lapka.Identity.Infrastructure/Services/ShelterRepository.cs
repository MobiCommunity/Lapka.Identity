using System.Collections.Generic;
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
    }
    
}