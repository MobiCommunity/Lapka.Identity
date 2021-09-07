using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Auths;

namespace Lapka.Identity.Application.Services
{
    public interface IFacebookAuthenticator
    {
        Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken);

        Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken);
    }
}