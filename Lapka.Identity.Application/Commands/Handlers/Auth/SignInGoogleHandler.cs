using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services.Auth;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class SignInGoogleHandler : ICommandHandler<SignInGoogle>
    {
        private readonly IIdentityService _identityService;

        public SignInGoogleHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task HandleAsync(SignInGoogle command) => await _identityService.SignInByGoogleAsync(command);
    }
}