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
        public async Task<int> GetShelterPetCountAsync(Guid shelterId)
        {
            GetShelterPetsCountReply result = await _client.GetShelterPetsCountAsync(new GetShelterPetsCountRequest
            {
                ShelterId = shelterId.ToString()
            });

            return result.Count;
        }
    }
}