using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class DeleteUserHandler : ICommandHandler<DeleteUser>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
        }
        public async Task HandleAsync(DeleteUser command)
        {
            User user = await _userRepository.GetAsync(command.Id);

            if (user is null)
            {
                throw new UserNotFoundException(command.Id.ToString());
            }
            
            user.Delete();
            
            await _userRepository.DeleteAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}