using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services
{
    public interface IShelterRepository
    {
        Task AddAsync(Shelter shelter);

        Task<IEnumerable<Shelter>> GetAllAsync();
        Task DeleteAsync(Shelter shelter);
        Task UpdateAsync(Shelter shelter);
        Task<Shelter> GetByIdAsync(Guid id);
    }
}