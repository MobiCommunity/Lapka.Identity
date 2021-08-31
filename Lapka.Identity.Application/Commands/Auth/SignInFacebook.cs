namespace Lapka.Identity.Application.Commands
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