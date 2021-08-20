using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Convey.Auth;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetAsync(Guid id)
        {
            UserDocument user = await _repository.GetAsync(id);

            return user?.AsBusiness();
        }

        public async Task<User> GetAsync(string email)
        {
            UserDocument user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

            return user?.AsBusiness();
        }

        public Task AddAsync(User user) => _repository.AddAsync(user.AsDocument());
    }
}