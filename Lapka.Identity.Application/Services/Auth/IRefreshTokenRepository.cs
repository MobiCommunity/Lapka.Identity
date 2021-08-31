using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services.Auth
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
    }
}