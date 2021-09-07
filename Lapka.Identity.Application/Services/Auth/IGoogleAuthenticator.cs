using System.Threading.Tasks;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Services.Auth
{
    public interface IGoogleAuthenticator
    {
        Task<GoogleUser> GetUserInfoAsync(string accessToken);
    }
}