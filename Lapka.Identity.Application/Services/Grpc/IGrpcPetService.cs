using System;
using System.Threading.Tasks;

namespace Lapka.Identity.Application.Services.Grpc
{
    public interface IGrpcPetService
    {
        Task<int> GetShelterPetCountAsync(Guid shelterId);
    }
}