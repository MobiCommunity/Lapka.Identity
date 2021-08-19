namespace Lapka.Identity.Core.ValueObjects
{
    public class JsonWebToken
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public long Expires { get; }

        public JsonWebToken(string accessToken, string refreshToken, long expires)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Expires = expires;
        }
    }
}