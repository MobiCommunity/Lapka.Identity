using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Services.Repositories
{
    public interface IShelterViewsRepository
    {
        Task<ShelterViews> GetByIdAsync(Guid id);
        Task AddAsync(ShelterViews view);
        Task UpdateAsync(ShelterViews view);
        Task DeleteAsync(ShelterViews view);
        Task DeleteAsync(Guid id);
    }
}