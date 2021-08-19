using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Token
{
    public class InvalidRefreshTokenException : DomainException
    {
        public InvalidRefreshTokenException() : base("Refresh token is invalid")
        {
        }

        public override string Code => "invalid_refresh_token";
    }
}