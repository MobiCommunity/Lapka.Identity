using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Shelter;

namespace Lapka.Identity.Application.Commands.Handlers.Shelter
{
    public class DeleteShelterHandler : ICommandHandler<DeleteShelter>
    {
        private readonly IShelterRepository _shelterRepository;
        private readonly IEventProcessor _eventProcessor;

        public DeleteShelterHandler(IShelterRepository shelterRepository, IEventProcessor eventProcessor)
        {
            _shelterRepository = shelterRepository;
            _eventProcessor = eventProcessor;
        }
        public async Task HandleAsync(DeleteShelter command)
        {
            Core.Entities.Shelter shelter = await _shelterRepository.GetByIdAsync(command.Id);
            if (shelter is null)
            {
                throw new ShelterNotFoundException();
            }
            
            shelter.Delete();
            
            await _shelterRepository.DeleteAsync(shelter);
            
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}