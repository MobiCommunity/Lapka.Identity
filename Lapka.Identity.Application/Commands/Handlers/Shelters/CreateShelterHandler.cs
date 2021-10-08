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
using Lapka.Identity.Core.Entities;
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
            if (command.UserAuth.Role != UserRoles.Admin.ToString())
            {
                throw new Application.Exceptions.UnauthorizedAccessException();
            }

            Shelter created = Shelter.Create(command.Id, command.Name, command.Address, command.GeoLocation,
                new PhoneNumber(command.PhoneNumber), new EmailAddress(command.Email),
                new BankNumber(command.BankNumber));

            string path = await AddPhotoAsync(command);
            created.UpdatePhoto(path, "");

            await _shelterRepository.AddAsync(created);
            await _eventProcessor.ProcessAsync(created.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(created.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        private async Task<string> AddPhotoAsync(CreateShelter command)
        {
            string path = String.Empty;
            try
            {
                path = await _grpcPhotoService.AddAsync(command.Photo.Name, command.UserAuth.UserId, true,
                    command.Photo.Content, BucketName.ShelterPhotos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return path;
        }
    }
}