using System;
using System.Threading.Tasks;

namespace Lapka.Identity.Application.Services
{
    public interface IGrpcPetService
    {
        Task CreatePetListsRequest(Guid userId);
        Task DeletePetListsRequest(Guid userId);
    }
}