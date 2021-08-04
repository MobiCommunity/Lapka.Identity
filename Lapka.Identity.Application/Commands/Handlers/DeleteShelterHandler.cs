﻿using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Application.Commands.Handlers
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
            var shelter = await _shelterRepository.GetByIdAsync(command.Id);
            shelter.Delete();
            
            await _shelterRepository.DeleteAsync(shelter);
            
            await _eventProcessor.ProcessAsync(shelter.Events);
        }
    }
}