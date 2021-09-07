namespace Lapka.Identity.Application.Commands.Auth
{
    public class SignInFacebook
    {
        public SignInFacebook(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}