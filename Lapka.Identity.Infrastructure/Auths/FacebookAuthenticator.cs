using System;
using System.Net.Http;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto.Auths;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Infrastructure.Documents;
using Newtonsoft.Json;

namespace Lapka.Identity.Infrastructure.Auths
{
    public class FacebookAuthenticator : IFacebookAuthenticator
    {
        private readonly FacebookAuthSettings _facebookAuthSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public FacebookAuthenticator(FacebookAuthSettings facebookAuthSettings, IHttpClientFactory httpClientFactory)
        {
            _facebookAuthSettings = facebookAuthSettings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<FacebookTokenValidationResult> ValidateAccessTokenAsync(string accessToken)
        {
            string formattedUrl = string.Format(_facebookAuthSettings.TokenValidationUrl, accessToken, _facebookAuthSettings.AppId,
                _facebookAuthSettings.AppSecret);

            HttpResponseMessage result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            try
            {
                result.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw new InvalidAccessTokenException(accessToken);
            }

            string responseAtString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookTokenValidationResult>(responseAtString);
        }

        public async Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken)
        {
            string formattedUrl = string.Format(_facebookAuthSettings.UserInfoUrl, accessToken);

            HttpResponseMessage result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();

            string responseAtString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FacebookUserInfoResult>(responseAtString);
        }
    }
}