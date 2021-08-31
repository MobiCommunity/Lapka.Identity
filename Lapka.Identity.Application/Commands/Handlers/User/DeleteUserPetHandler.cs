using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class DeleteUserPetHandler : ICommandHandler<DeleteUserPet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;

        public DeleteUserPetHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
        }
        public async Task HandleAsync(DeleteUserPet command)
        {
            User user = await _userRepository.GetAsync(command.UserId);
            user.DeletePet(command.PetId);
            
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}