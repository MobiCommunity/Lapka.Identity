using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Application.Commands.Handlers.Auth
{
    public class SignUpHandler : ICommandHandler<SignUp>
    {
        private readonly IIdentityService _identityService;

        public SignUpHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task HandleAsync(SignUp command) => await _identityService.SignUpAsync(command);
    }
}