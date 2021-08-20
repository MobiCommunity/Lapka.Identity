using Convey.CQRS.Commands;

namespace Lapka.Identity.Application.Commands
{
    public class RevokeRefreshToken : ICommand
    {
        public string Token { get; }

        public RevokeRefreshToken(string token)
        {
            Token = token;
        }
    }
}