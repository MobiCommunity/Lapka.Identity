using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions
{
    public class InvalidAccessTokenException : DomainException
    {
        public string AccessToken { get; }
        public InvalidAccessTokenException(string token) : base($"Invalid access token: {token}")
        {
            AccessToken = token;
        }

        public override string Code => "invalid_access_token";
    }
}