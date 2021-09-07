using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Core.Exceptions;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Options;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Lapka.Identity.Infrastructure.Auth
{
    public class GoogleAuthenticator : IGoogleAuthenticator
    {
        private readonly GoogleAuthSettings _googleAuthSettings;

        public GoogleAuthenticator(GoogleAuthSettings googleAuthSettings)
        {
            _googleAuthSettings = googleAuthSettings;
        }

        public async Task<GoogleUser> GetUserInfoAsync(string accessToken)
        {
            Payload payload;
            try
            {
                payload = await ValidateAsync(accessToken,
                    new ValidationSettings
                    {
                        Audience = new[] {_googleAuthSettings.ClientId}
                    });
            }
            catch(Exception)
            {
                throw new InvalidAccessTokenException(accessToken);
            }

            GoogleUser user = new GoogleUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = payload.Email,
                FamilyName = payload.FamilyName,
                GivenName = payload.Name,
                Name = payload.Email,
                Picture = payload.Picture
            };

            return user;
        }
    }
}