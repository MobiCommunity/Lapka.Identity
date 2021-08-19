using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class SignUpHandler : ICommandHandler<SignUp>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IIdentityService _identityService;

        public SignUpHandler(IEventProcessor eventProcessor, IIdentityService identityService)
        {
            _eventProcessor = eventProcessor;
            _identityService = identityService;
        }

        public async Task HandleAsync(SignUp command)
        {
            User user = new User(command.Id, command.Username, command.FirstName, command.LastName, command.Email,
                command.Password, command.PhoneNumber, command.PhotoPath, command.CreatedAt, command.Role);
            
            await _identityService.SignUpAsync(command);
            await _eventProcessor.ProcessAsync(user.Events);
        } 
    }
}