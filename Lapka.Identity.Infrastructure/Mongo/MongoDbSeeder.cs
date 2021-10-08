using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Lapka.Identity.Infrastructure.Mongo
{
    public class MongoDbSeeder : IMongoDbSeeder
    {
        public async Task SeedAsync(IMongoDatabase database)
        {
            IMongoCollection<UserDocument> collection = database.GetCollection<UserDocument>("users");
            IMongoQueryable<UserDocument> users = collection.AsQueryable();
            users = users.Where(x => x.Role == UserRoles.Admin.ToString());
            if (users.Any())
            {
                return;
            }
            
            UserDocument user = new UserDocument
            {
                Id = Guid.NewGuid(),
                Username = UserRoles.Admin.ToString(),
                FirstName = UserRoles.Admin.ToString(),
                LastName = UserRoles.Admin.ToString(),
                Email = "admin@admin.com",
                Password = "AQAAAAEAACcQAAAAEFMWjVmxMPfX0qlQHDPRGQn1TanD8xL7u7p+iBjTdn4pOfOeaXZmuwYVRE+/mfrmZw==", //admin
                CreatedAt = DateTime.UtcNow,
                Role = UserRoles.Admin.ToString()
            };
            await collection.InsertOneAsync(user);
        }
    }
}