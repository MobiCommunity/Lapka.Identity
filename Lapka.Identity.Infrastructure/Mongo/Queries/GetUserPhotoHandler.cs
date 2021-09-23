using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Mongo.Queries
{
    public class GetUserPhotoHandler : IQueryHandler<GetUserPhoto, string>
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public GetUserPhotoHandler(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<string> HandleAsync(GetUserPhoto query)
        {
            UserDocument shelter = await GetUserAsync(query);
            
            return shelter.PhotoPath;
        }

        private async Task<UserDocument> GetUserAsync(GetUserPhoto query)
        {
            UserDocument user = await _repository.GetAsync(query.Id);
            if (user is null)
            {
                throw new UserNotFoundException(query.Id.ToString());
            }
            
            return user;
        }
    }
}