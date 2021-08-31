using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services.Shelter
{
    public interface IShelterRepository
    {
        Task AddAsync(Core.Entities.Shelter shelter);

        Task<IEnumerable<Core.Entities.Shelter>> GetAllAsync();
        Task DeleteAsync(Core.Entities.Shelter shelter);
        Task UpdateAsync(Core.Entities.Shelter shelter);
        Task<Core.Entities.Shelter> GetByIdAsync(Guid id);
    }
}