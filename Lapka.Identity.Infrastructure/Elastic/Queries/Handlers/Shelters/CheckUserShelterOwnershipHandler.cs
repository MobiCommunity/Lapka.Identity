using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Shelters
{
    public class CheckUserShelterOwnershipHandler : IQueryHandler<CheckUserShelterOwnership, bool>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public CheckUserShelterOwnershipHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<bool> HandleAsync(CheckUserShelterOwnership query)
        {
            ShelterDocument shelter = await GetShelterAsync(query);

            return shelter.Owners.Any(x => x == query.UserId);
        }

        private async Task<ShelterDocument> GetShelterAsync(CheckUserShelterOwnership query)
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