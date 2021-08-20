using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
    }
}