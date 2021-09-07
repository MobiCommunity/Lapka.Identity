using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Handlers;
using Lapka.Identity.Application.Commands.Handlers.Users;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using NSubstitute;
using Xunit;

namespace Lapka.Identity.Tests.Unit.Application.Handlers.UserTests
{
    public class UpdateUserPhotoHandlerTests
    {
        private readonly UpdateUserPhotoHandler _handler;
        private readonly IEventProcessor _eventProcessor;
        private readonly IGrpcPhotoService _photoService;
        private readonly IUserRepository _userRepository;

        public UpdateUserPhotoHandlerTests()
        {
            _eventProcessor = Substitute.For<IEventProcessor>();
            _photoService = Substitute.For<IGrpcPhotoService>();
            _userRepository = Substitute.For<IUserRepository>();
            _handler = new UpdateUserPhotoHandler(_eventProcessor, _userRepository, _photoService);
        }

        private Task Act(UpdateUserPhoto command)
        {
            return _handler.HandleAsync(command);
        }

        [Fact]
        public async Task given_valid_user_should_update()
        {
            Guid photoId = Guid.NewGuid();
            User arrangeUser = Extensions.ArrangeUser();
            Guid oldPhotoId = arrangeUser.PhotoId;
            PhotoFile file = Extensions.ArrangePhotoFile(photoId);
            
            User user = User.Create(arrangeUser.Id.Value, arrangeUser.Username, arrangeUser.FirstName,
                arrangeUser.LastName, arrangeUser.Email, arrangeUser.Password, arrangeUser.CreatedAt, arrangeUser.Role);

            UpdateUserPhoto command = new UpdateUserPhoto(user.Id.Value, file);
            _userRepository.GetAsync(command.UserId).Returns(user);

            await Act(command);

            await _userRepository.Received().UpdateAsync(user);
            await _photoService.DeleteAsync(oldPhotoId, BucketName.UserPhotos);
            await _photoService.AddAsync(file.Id, file.Name, file.Content, BucketName.UserPhotos);
            await _eventProcessor.Received().ProcessAsync(user.Events);        }
    }
}