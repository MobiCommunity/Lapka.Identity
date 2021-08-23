using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions.Identity;
using Lapka.Identity.Core.Exceptions.User;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
        }
        public async Task HandleAsync(UpdateUser command)
        {
            User user = await _userRepository.GetAsync(command.Id);
            if (user is null)
            {
                throw new UserNotFoundException(command.Id.ToString());
            }
            
            user.Update(command.Username, command.FirstName, command.LastName, user.Email, command.PhoneNumber, user.Role,
                user.PhotoPath);

            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}