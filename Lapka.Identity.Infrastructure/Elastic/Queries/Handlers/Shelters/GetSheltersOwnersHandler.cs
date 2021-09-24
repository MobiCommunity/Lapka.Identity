using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Shelters
{
    public class GetSheltersOwnersHandler : IQueryHandler<GetSheltersOwners, IEnumerable<Guid>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetSheltersOwnersHandler(IElasticClient elasticClient, ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }
        
        public async Task<IEnumerable<Guid>> HandleAsync(GetSheltersOwners query)
        {
            ShelterDocument shelter = await GetShelterDocumentAsync(query);

            IEnumerable<Guid> ownersId = shelter.Owners.ToList();

            return ownersId;
        }

        private async Task<ShelterDocument> GetShelterDocumentAsync(GetSheltersOwners query)
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