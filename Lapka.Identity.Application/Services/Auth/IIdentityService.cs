using System.Threading.Tasks;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Services.Auth
{
    public interface IIdentityService
    {
        Task<AuthDto> SignInAsync(SignIn command);
        Task<AuthDto> SignInByGoogleAsync(SignInGoogle command);
        Task SignUpAsync(SignUp command);
        Task<AuthDto> FacebookLoginAsync(SignInFacebook command);
        Task ChangeUserPasswordAsync(UpdateUserPassword command);

    }
}