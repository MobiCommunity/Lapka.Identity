using System;
using System.Collections.Generic;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Services.Auth
{
    public interface IJwtProvider
    {
        AuthDto Create(Guid userId, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null);
    }
}