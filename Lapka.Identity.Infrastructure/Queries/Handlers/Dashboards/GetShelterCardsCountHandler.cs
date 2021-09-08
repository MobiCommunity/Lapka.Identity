using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers.Dashboards
{
    public class GetShelterCardsCountHandler : IQueryHandler<GetShelterCardsCount, int>
    {
        private readonly IMongoRepository<ShelterDocument, Guid> _repository;
        private readonly IGrpcPetService _petService;

        public GetShelterCardsCountHandler(IMongoRepository<ShelterDocument, Guid> repository,
            IGrpcPetService petService)
        {
            _repository = repository;
            _petService = petService;
        }

        public async Task<int> HandleAsync(GetShelterCardsCount query)
        {
            ShelterDocument shelter = await _repository.GetAsync(x => x.Id == query.ShelterId);
            if (shelter.Owners.Any(x => x != query.Auth.UserId) && query.Auth.Role != "admin")
            {
                throw new Application.Exceptions.UnauthorizedAccessException();
            }

            int petCount = 0;
            try
            {
                petCount = await _petService.GetShelterPetCountAsync(query.ShelterId);
            }
            catch (Exception ex)
            {
                throw new CannotRequestPetsMicroserviceException(ex);
            }

            return petCount;
        }
    }
}