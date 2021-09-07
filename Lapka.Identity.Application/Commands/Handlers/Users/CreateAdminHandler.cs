using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;

namespace Lapka.Identity.Application.Commands.Handlers.Users
{
    public class CreateAdminHandler : ICommandHandler<CreateAdmin>
    {
        private readonly IIdentityService _identityService;

        public CreateAdminHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task HandleAsync(CreateAdmin command)
        {
            await _identityService.SignUpAsync(new SignUp(Guid.NewGuid(), "admin", "admin",
                "admin", "admin@admin.com", "admin", DateTime.Now, "admin"));
        }
    }
}