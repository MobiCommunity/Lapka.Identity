using System;
using System.Threading.Tasks;


namespace Lapka.Identity.Application.Services.User
{
    public interface IUserRepository
    {
        Task<Core.Entities.User> GetAsync(Guid id);
        Task<Core.Entities.User> GetAsync(string email);
        Task AddAsync(Core.Entities.User user);
    }
}