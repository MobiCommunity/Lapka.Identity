using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services
{
    public interface IShelterRepository
    {
        Task AddAsync(Shelter shelter);
        Task DeleteAsync(Shelter shelter);
        Task UpdateAsync(Shelter shelter);
        Task<Shelter> GetByIdAsync(Guid id);
    }
}