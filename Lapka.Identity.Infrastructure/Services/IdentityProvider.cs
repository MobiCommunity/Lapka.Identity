using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Infrastructure.Services
{
    public class IdValueProvider : IIdValueProvider
    {
        public Guid GetIdValue(HttpContext ctx) 
            => Guid.Parse(GetIdValueAsString(ctx));

        public string GetIdValueAsString(HttpContext ctx) 
            => ctx.Request.Headers["Authorization"].FirstOrDefault();

    }
}