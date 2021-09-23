using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Grpc.Core;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;

namespace Lapka.Identity.Api.Grpc.Controllers
{
    public class GrpcShelterController : ShelterProto.ShelterProtoBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public GrpcShelterController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }
        public override async Task<GetShelterLocationReply> GetShelterLocation(GetShelterLocationRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.ShelterId, out Guid shelterId))
            {
                throw new InvalidShelterIdException(request.ShelterId);
            }
            
            ShelterDto shelter = await _queryDispatcher.QueryAsync(new GetShelterElastic
            {
                Id = shelterId
            });

            return new GetShelterLocationReply
            {
                Longitude = shelter.GeoLocation.Longitude.ToString(),
                Latitude = shelter.GeoLocation.Latitude.ToString()
            };
        }

        public override async Task<GetShelterBasicInfoReply> GetShelterBasicInfo(GetShelterBasicInfoRequest request, ServerCallContext context)
        {
            if(!Guid.TryParse(request.ShelterId, out Guid shelterId))
            {
                throw new InvalidShelterIdException(request.ShelterId);
            }
            
            ShelterDto shelter = await _queryDispatcher.QueryAsync(new GetShelterElastic
            {
                Id = shelterId
            });

            return new GetShelterBasicInfoReply
            {
                Name = shelter.Name,
                Longitude = shelter.GeoLocation.Longitude.ToString(),
                Latitude = shelter.GeoLocation.Latitude.ToString(),
                Street = shelter.Address.Street,
                ZipCode = shelter.Address.ZipCode,
                City = shelter.Address.City,
            };
        }
    }
}