using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;

namespace Lapka.Identity.Api.Grpc.Controllers
{
    public class GrpcIdentityPhotoController : IdentityPhotoProto.IdentityPhotoProtoBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GrpcIdentityPhotoController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<GetShelterPhotoReply> GetShelterPhoto(GetShelterPhotoRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.ShelterId, out Guid shelterId))
            {
                throw new InvalidShelterIdException(request.ShelterId);
            }
            
            string shelterPhotoPath = await _queryDispatcher.QueryAsync(new GetShelterPhoto
            {
                Id = shelterId
            });

            return new GetShelterPhotoReply
            {
                Path = shelterPhotoPath
            };
        }

        public override async Task<GetUserPhotoReply> GetUserPhoto(GetUserPhotoRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.UserId, out Guid userId))
            {
                throw new InvalidShelterIdException(request.UserId);
            }
            
            string userPhotoPath = await _queryDispatcher.QueryAsync(new GetUserPhoto
            {
                Id = userId
            });

            return new GetUserPhotoReply
            {
                Path = userPhotoPath
            };
        }
    }
}