using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services
{
    public interface IShelterRepository
    {
        Task AddAsync(Shelter shelter);

        Task<IEnumerable<Shelter>> GetAllAsync();
    }
}