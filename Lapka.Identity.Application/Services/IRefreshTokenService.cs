using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(Guid userId);
        Task RevokeAsync(string refreshToken);
        Task<AuthDto> UseAsync(string refreshToken);
    }
}