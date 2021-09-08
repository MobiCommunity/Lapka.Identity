using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;

namespace Lapka.Identity.Application.Commands.Handlers.Users
{
    public class UpdateUserPasswordHandler : ICommandHandler<UpdateUserPassword>
    {
        private readonly IIdentityService _identityService;

        public UpdateUserPasswordHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task HandleAsync(UpdateUserPassword command)
        {
            await _identityService.ChangeUserPasswordAsync(command);
        }
    }
}