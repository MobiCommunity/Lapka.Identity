using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers.Shelters
{
    public class CreateShelterHandler : ICommandHandler<CreateShelter>
    {
        private readonly ILogger<CreateShelterHandler> _logger;
        private readonly IShelterRepository _shelterRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IEventProcessor _eventProcessor;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;

        public CreateShelterHandler(ILogger<CreateShelterHandler> logger, IShelterRepository shelterRepository,
            IGrpcPhotoService grpcPhotoService, IEventProcessor eventProcessor, IMessageBroker messageBroker,
            IDomainToIntegrationEventMapper eventMapper)
        {
            _logger = logger;
            _shelterRepository = shelterRepository;
            _grpcPhotoService = grpcPhotoService;
            _eventProcessor = eventProcessor;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(CreateShelter command)
        {
            if (command.UserAuth.Role != "admin")
            {
                throw new Application.Exceptions.UnauthorizedAccessException();
            }

            Core.Entities.Shelter created = Core.Entities.Shelter.Create(command.Id, command.Name, command.Address,
                command.GeoLocation, command.Photo.Id, command.PhoneNumber, command.Email, command.BankNumber,
                new List<Guid>());

            await _shelterRepository.AddAsync(created);
            await AddPhotoAsync(command, created);

            await _eventProcessor.ProcessAsync(created.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(created.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        private async Task AddPhotoAsync(CreateShelter command, Core.Entities.Shelter created)
        {
            try
            {
                await _grpcPhotoService.AddAsync(command.Photo.Id, command.Photo.Name, command.Photo.Content,
                    BucketName.ShelterPhotos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                created.UpdatePhoto(Guid.Empty);

                await _shelterRepository.UpdateAsync(created);
            }
        }
    }
}