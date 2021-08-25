using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Application.Commands.Handlers.Auth
{
    public class SignInHandler : ICommandHandler<SignIn>
    {
        private readonly IIdentityService _identityService;

        public SignInHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task HandleAsync(SignIn command) => await _identityService.SignInAsync(command);
    }
}