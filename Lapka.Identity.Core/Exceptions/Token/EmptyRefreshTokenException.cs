using Lapka.Identity.Core.Exceptions.Abstract;

namespace Lapka.Identity.Core.Exceptions.Token
{
    public class EmptyRefreshTokenException : DomainException
    {
        public string RefreshToken { get; }
        public EmptyRefreshTokenException(string refreshToken ) : base($"Invalid refresh token: {refreshToken}")
        {
            RefreshToken = refreshToken;
        }

        public override string Code => "invalid_refresh_token";
    }
}