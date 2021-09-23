using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
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
            users = users.Where(x => x.Role == "admin");
            if (users.Any())
            {
                return;
            }
            
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