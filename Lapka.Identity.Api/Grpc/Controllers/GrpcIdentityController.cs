using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;

namespace Lapka.Identity.Api.Grpc.Controllers
{
    public class GrpcIdentityController : IdentityProto.IdentityProtoBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GrpcIdentityController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public override async Task<GetClosestShelterReply> GetClosestShelter(GetClosestShelterRequest request, ServerCallContext context)
        {
            ShelterDto shelter = await _queryDispatcher.QueryAsync(new GetClosestShelter
            {
                Longitude = request.Longitude,
                Latitude = request.Latitude
            });

            return new GetClosestShelterReply
            {
                ShelterId = shelter.Id.ToString()
            };
        }

        public override async Task<IsUserOwnerOfShelterReply> IsUserOwnerOfShelter(IsUserOwnerOfShelterRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.ShelterId, out Guid shelterId) || !Guid.TryParse(request.UserId, out Guid userId))
            {
                return new IsUserOwnerOfShelterReply
                {
                    IsOwner = false
                }; 
            }

            return new IsUserOwnerOfShelterReply
            {
                IsOwner = await _queryDispatcher.QueryAsync(new CheckUserShelterOwnership
                {
                    ShelterId = shelterId,
                    UserId = userId
                })
            };        
        }

        public override async Task<DoesShelterExistsReply> DoesShelterExists(DoesShelterExistsRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.ShelterId, out Guid shelterId))
            {
                throw new InvalidShelterIdException(request.ShelterId);
            }

            bool isExists = false;

            try
            {
                ShelterDto shelter = await _queryDispatcher.QueryAsync(new GetShelter
                {
                    Id = shelterId
                });

                if (shelter is { })
                    isExists = true;
            }
            catch
            {
                isExists = false;
            }

            return new DoesShelterExistsReply
            {
                DoesExists = isExists
            };  
        }
    }
}