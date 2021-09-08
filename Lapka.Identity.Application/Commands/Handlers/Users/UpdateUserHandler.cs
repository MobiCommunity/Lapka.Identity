using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Commands.Handlers.Users
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
            if (user is null) throw new UserNotFoundException(command.Id.ToString());

            user.Update(command.Username, command.FirstName, command.LastName, command.PhoneNumber);

            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}