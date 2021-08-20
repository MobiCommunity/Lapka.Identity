using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Services.Auth
{
    public interface IIdentityService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<AuthDto> SignInAsync(SignIn command);
        Task SignUpAsync(SignUp command);
    }
}