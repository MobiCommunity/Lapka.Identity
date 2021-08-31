using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.User;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Infrastructure.Documents;
using Lapka.Identity.Infrastructure.Exceptions;

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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            IReadOnlyList<UserDocument> users = await _repository.FindAsync(_ => true);

            return users.Select(x => x.AsBusiness());
        }

        public async Task<User> GetAsync(string email)
        {
            UserDocument user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

            return user?.AsBusiness();
        }

        public async Task AddAsync(User user) => await _repository.AddAsync(user.AsDocument());
        public async Task UpdateAsync(User user)
        {
            await _repository.UpdateAsync(user.AsDocument());
            
        }
        public async Task DeleteAsync(User user)
        {
            await _repository.DeleteAsync(user.Id.Value);
            
        }
    }
}