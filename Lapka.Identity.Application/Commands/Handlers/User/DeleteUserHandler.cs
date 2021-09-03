using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class DeleteUserHandler : ICommandHandler<DeleteUser>
    {
        private readonly ILogger<DeleteUserHandler> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IUserRepository _userRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public DeleteUserHandler(ILogger<DeleteUserHandler> logger, IEventProcessor eventProcessor,
            IUserRepository userRepository, IGrpcPhotoService grpcPhotoService)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;
            _userRepository = userRepository;
            _grpcPhotoService = grpcPhotoService;
        }

        public async Task HandleAsync(DeleteUser command)
        {
            User user = await _userRepository.GetAsync(command.Id);

            if (user is null)
            {
                throw new UserNotFoundException(command.Id.ToString());
            }

            try
            {
                if (user.PhotoId != Guid.Empty)
                {
                    await _grpcPhotoService.DeleteAsync(user.PhotoId, BucketName.UserPhotos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Did not deleted user photo with id: {user.PhotoId}");
            }

            user.Delete();

            await _userRepository.DeleteAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}