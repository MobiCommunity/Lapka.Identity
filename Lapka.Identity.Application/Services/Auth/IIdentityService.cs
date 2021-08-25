using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Services
{
    public interface IIdentityService
    {
        Task<AuthDto> SignInAsync(SignIn command);
        Task SignUpAsync(SignUp command);
        Task<AuthDto> FacebookLoginAsync(SignInFacebook command);
        Task ChangeUserPasswordAsync(UpdateUserPassword command);

    }
}