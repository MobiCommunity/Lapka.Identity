using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Token
{
    public class RevokedRefreshTokenException : DomainException
    {
        public RevokedRefreshTokenException() : base("Refresh token is revoked")
        {
        }

        public override string Code => "revoked_refresh_token";
    }
}