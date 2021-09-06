using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.UserTests
{
    public class DeleteUserHandlerTests
    {
        private readonly DeleteUserHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<DeleteUserHandler> _logger;
        private readonly IGrpcPhotoService _photoService;

        public DeleteUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _logger = Substitute.For<ILogger<DeleteUserHandler>>();
            _handler = new DeleteUserHandler(_logger, _eventProcessor, _userRepository, _photoService);
        }

        private Task Act(DeleteUser command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_user_should_update()
        {
            User arrangeUser = Extensions.ArrangeUser();

            User user = User.Create(arrangeUser.Id.Value, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, arrangeUser.CreatedAt, arrangeUser.Role);

            DeleteUser command = new DeleteUser(user.Id.Value);
            _userRepository.GetAsync(command.Id).Returns(user);

            await Act(command);
            
            await _userRepository.Received().DeleteAsync(user);
            await _photoService.DeleteAsync(user.PhotoId, BucketName.UserPhotos);
            await _eventProcessor.Received().ProcessAsync(user.Events);        }
    }
}