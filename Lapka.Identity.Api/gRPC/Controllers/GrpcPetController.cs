using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Grpc.Core;
using Lapka.Identity.Application.Commands;

namespace Lapka.Pets.Api.gRPC.Controllers
{
    public class GrpcPetController : PetGrpc.PetGrpcBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public GrpcPetController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        public override async Task<UploadUserPetResponse> AddPetToUser(UploadUserPetRequest request, ServerCallContext context)
        {
            await _commandDispatcher.SendAsync(new AddUserPet(Guid.Parse(request.UserId), Guid.Parse(request.PetId)));

            return new UploadUserPetResponse();
        }

        public override async Task<DeleteUserPetResponse> DeletePetFromUser(DeleteUserPetRequest request, ServerCallContext context)
        {
            await _commandDispatcher.SendAsync(new DeleteUserPet(Guid.Parse(request.UserId), Guid.Parse(request.PetId)));
            
            return new DeleteUserPetResponse();
        }
    }
}