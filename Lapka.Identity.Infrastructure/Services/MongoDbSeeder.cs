using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Infrastructure.Documents;
using MongoDB.Driver;

namespace Lapka.Identity.Infrastructure.Services
{
    public class MongoDbSeeder : IMongoDbSeeder
    {
        public async Task SeedAsync(IMongoDatabase database)
        {
            IMongoCollection<UserDocument> collection = database.GetCollection<UserDocument>("users");
            UserDocument user = new UserDocument
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                Password = "AQAAAAEAACcQAAAAEFMWjVmxMPfX0qlQHDPRGQn1TanD8xL7u7p+iBjTdn4pOfOeaXZmuwYVRE+/mfrmZw==", //admin
                CreatedAt = DateTime.UtcNow,
                Role = "admin"
            };
            await collection.InsertOneAsync(user);
        }
    }
}