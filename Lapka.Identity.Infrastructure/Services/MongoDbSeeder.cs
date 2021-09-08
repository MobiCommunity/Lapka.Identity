using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Services.Auth;
using MongoDB.Driver;

namespace Lapka.Identity.Infrastructure.Services
{
    public class MongoDbSeeder : IMongoDbSeeder
    {
        private readonly IIdentityService _identityService;
        
        public MongoDbSeeder(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        //Doesnt work.
        [Obsolete]
        public async Task SeedAsync(IMongoDatabase database)
        {
            await _identityService.SignUpAsync(new SignUp(Guid.NewGuid(), "admin", "admin",
                "admin", "admin@admin.com", "admin", DateTime.Now, "admin"));
        }
    }
}