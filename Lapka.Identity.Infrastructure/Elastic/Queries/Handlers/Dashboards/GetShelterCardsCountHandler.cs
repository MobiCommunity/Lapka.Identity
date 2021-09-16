using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Exceptions.Grpc;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Application.Services.Grpc;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Dashboards
{
    public class GetShelterCardsCountHandler : IQueryHandler<GetShelterCardsCount, int>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;
        private readonly IGrpcPetService _petService;

        public GetShelterCardsCountHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions, IGrpcPetService petService)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
            _petService = petService;
        }

        public async Task<int> HandleAsync(GetShelterCardsCount query)
        {
            ShelterDocument shelter = await GetShelterAsync(query);

            ValidIfUserIsAccessibleToManageShelter(query, shelter);

            int petCount = await GetShelterPetCountAsync(query);

            return petCount;
        }

        private async Task<int> GetShelterPetCountAsync(GetShelterCardsCount query)
        {
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

        private static void ValidIfUserIsAccessibleToManageShelter(GetShelterCardsCount query, ShelterDocument shelter)
        {
            if (shelter.Owners.Any(x => x != query.Auth.UserId) && query.Auth.Role != "admin")
            {
                throw new Application.Exceptions.UnauthorizedAccessException();
            }
        }

        private async Task<ShelterDocument> GetShelterAsync(GetShelterCardsCount query)
        {
            GetResponse<ShelterDocument> response = await _elasticClient.GetAsync(
                new DocumentPath<ShelterDocument>(new Id(query.ShelterId.ToString())),
                x => x.Index(_elasticSearchOptions.Aliases.Shelters));

            ShelterDocument shelter = response?.Source;
            if (shelter is null)
            {
                throw new ShelterNotFoundException(query.ShelterId.ToString());
            }

            return shelter;
        }
    }
}