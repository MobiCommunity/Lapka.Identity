using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lapka.Identity.Application.Services.Repositories
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