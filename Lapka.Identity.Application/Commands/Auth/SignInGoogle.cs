using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.Auth
{
    public class SignInGoogle : ICommand
    {
        public SignInGoogle(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}