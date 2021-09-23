using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Services.Repositories
{
    public interface IShelterOwnerApplicationRepository
    {
        Task<ShelterOwnerApplication> GetAsync(Guid id);
        Task<IEnumerable<ShelterOwnerApplication>> GetAllAsync(Guid userId, Guid shelterId);
        Task<IEnumerable<ShelterOwnerApplication>> GetApplicationsMadeForShelterAsync(Guid shelterId);
        Task AddApplicationAsync(ShelterOwnerApplication application);
        Task UpdateAsync(ShelterOwnerApplication application);
    }
}