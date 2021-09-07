using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands.Auth
{
    public class RevokeRefreshToken : ICommand
    {
        public RevokeRefreshToken(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}