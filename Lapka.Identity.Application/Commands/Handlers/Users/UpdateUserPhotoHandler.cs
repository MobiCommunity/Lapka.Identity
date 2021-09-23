using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Grpc;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers.Users
{
    public class UpdateUserPhotoHandler : ICommandHandler<UpdateUserPhoto>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _grpcPhotoService;
        private readonly IMessageBroker _messageBroker;
        private readonly IDomainToIntegrationEventMapper _eventMapper;
        private readonly IUserRepository _userRepository;

        public UpdateUserPhotoHandler(IEventProcessor eventProcessor, IUserRepository userRepository,
            IGrpcPhotoService grpcPhotoService, IMessageBroker messageBroker,
            IDomainToIntegrationEventMapper eventMapper)
        {
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _grpcPhotoService = grpcPhotoService;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(UpdateUserPhoto command)
        {
            User user = await GetUserAsync(command);

            string path = await AddNewPhotoToMinioAsync(command);

            user.UpdatePhoto(path, user.PhotoPath ?? "");
            
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
            IEnumerable<IEvent> events = _eventMapper.MapAll(user.Events);
            await _messageBroker.PublishAsync(events);
        }

        private async Task<string> AddNewPhotoToMinioAsync(UpdateUserPhoto command)
        {
            string path;
            try
            {
                path = await _grpcPhotoService.AddAsync(command.Photo.Name, command.UserId, true, command.Photo.Content,
                    BucketName.UserPhotos);
            }
            catch (Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }

            return path;
        }

        private async Task<User> GetUserAsync(UpdateUserPhoto command)
        {
            User user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId.ToString());
            }

            return user;
        }
    }
}