using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.Exceptions.User;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Commands.Handlers
{
    public class UpdateUserPhotoHandler : ICommandHandler<UpdateUserPhoto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGrpcPhotoService _grpcPhotoService;

        public UpdateUserPhotoHandler(IUserRepository userRepository, IGrpcPhotoService grpcPhotoService)
        {
            _userRepository = userRepository;
            _grpcPhotoService = grpcPhotoService;
        }
        public async Task HandleAsync(UpdateUserPhoto command)
        {
            string mainPhotoPath = $"{command.PhotoId:N}.{command.Photo.GetFileExtension()}"; 

            User user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId.ToString());
            }
            
            try
            {
                if(!string.IsNullOrEmpty(user.PhotoPath))
                    await _grpcPhotoService.DeleteAsync(user.PhotoPath, BucketName.UserPhotos);
                
                await _grpcPhotoService.AddAsync(mainPhotoPath, command.Photo.Content, BucketName.UserPhotos);
            }
            catch(Exception ex)
            {
                throw new CannotRequestFilesMicroserviceException(ex);
            }
            
            user.Update(user.Username, user.FirstName, user.LastName, user.PhoneNumber, user.Role, mainPhotoPath);
            await _userRepository.UpdateAsync(user);
        }
    }
}