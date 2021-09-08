using System;
using System.Threading.Tasks;

namespace Lapka.Identity.Application.Services
{
    public interface IGrpcPetService
    {
        Task<int> GetShelterPetCountAsync(Guid shelterId);
    }
}