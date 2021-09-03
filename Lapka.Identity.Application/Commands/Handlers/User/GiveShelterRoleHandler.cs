using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class GiveShelterRoleHandler : ICommandHandler<GiveShelterRole>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;

        public GiveShelterRoleHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
        }
        
        public async Task HandleAsync(GiveShelterRole command)
        {
            User user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId.ToString());
            }
            
            user.Update(user.Username, user.FirstName, user.LastName, user.PhoneNumber, "shelter");

            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}