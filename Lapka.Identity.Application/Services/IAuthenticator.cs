using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Application.Services
{
    public interface IAuthenticator
    {
        public void Authenticate(HttpContext ctx);
    }
}