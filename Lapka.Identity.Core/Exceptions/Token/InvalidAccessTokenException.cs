using Lapka.Identity.Core.Events.Abstract;
using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Token
{
    public class InvalidAccessTokenException : DomainException
    {
        public string AccessToken { get; }
        
        public InvalidAccessTokenException(string accessToken) : base($"Invalid access token: {accessToken}")
        {
            AccessToken = accessToken;
        }

        public override string Code => "invalid_access_token";
    }
}