using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions.Identity;

namespace Lapka.Identity.Application.Commands.Handlers.Users
{
    public class UpdateUserEmailHandler : ICommandHandler<UpdateUserEmail>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;

        public UpdateUserEmailHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateUserEmail command)
        {
            User user = await _userRepository.GetAsync(command.Email.Value);
            if (user is { })
            {
                throw new EmailInUseException(command.Email.Value);
            }

            user = await _userRepository.GetAsync(command.Id);

            user.UpdateEmail(command.Email);

            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}