using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class CreateShelterHandler : ICommandHandler<CreateShelter>
    {

        private readonly IShelterRepository _shelterRepository;
        private readonly IEventProcessor _eventProcessor;

        public CreateShelterHandler(IShelterRepository shelterRepository, IEventProcessor eventProcessor)
        {
            _shelterRepository = shelterRepository;
            _eventProcessor = eventProcessor;
        }

        public async Task HandleAsync(CreateShelter command)
        {
            //TODO application layer validation
            
            Shelter created = Shelter.Create(command.Id, command.Name, command.Address, 
                command.GeoLocation, command.PhoneNumber, command.Email);

            await _shelterRepository.AddAsync(created);
            
            await _eventProcessor.ProcessAsync(created.Events);
            
        }
    }
}