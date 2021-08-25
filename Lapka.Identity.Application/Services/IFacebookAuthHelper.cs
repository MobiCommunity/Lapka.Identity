using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Services
{
    public interface IFacebookAuthHelper
    {
        Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken);

        Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken);
    }
}