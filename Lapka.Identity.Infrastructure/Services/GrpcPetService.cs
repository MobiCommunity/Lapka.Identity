using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Infrastructure.Services
{
    public class GrpcPetService : IGrpcPetService
    {
        private readonly PetProto.PetProtoClient _client;

        public GrpcPetService(PetProto.PetProtoClient client)
        {
            _client = client;
        }

        public async Task CreatePetListsRequest(Guid userId)
        {
            await _client.CreatePetListsAsync(new CreatePetListsRequest
            {
                UserId = userId.ToString()
            });
        }

        public async Task DeletePetListsRequest(Guid userId)
        {
            await _client.DeleteUserPetsAsync(new DeleteUserPetsRequest
            {
                UserId = userId.ToString()
            });
        }
    }
}