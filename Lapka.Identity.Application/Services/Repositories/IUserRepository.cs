using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lapka.Identity.Application.Services.Repositories
{
    public interface IUserRepository
    {
        Task<Core.Entities.User> GetAsync(Guid id);
        Task<IEnumerable<Core.Entities.User>> GetAllAsync();
        Task<Core.Entities.User> GetAsync(string email);
        Task AddAsync(Core.Entities.User user);
        Task UpdateAsync(Core.Entities.User user);
    }
}