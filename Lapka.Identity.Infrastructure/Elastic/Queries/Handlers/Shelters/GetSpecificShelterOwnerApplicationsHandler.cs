using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Shelters
{
    public class GetSpecificShelterOwnerApplicationsHandler : IQueryHandler<GetSpecificShelterOwnerApplications,
        IEnumerable<ShelterOwnerApplicationDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetSpecificShelterOwnerApplicationsHandler(IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<ShelterOwnerApplicationDto>> HandleAsync(
            GetSpecificShelterOwnerApplications query)
        {
            ICollection<ShelterOwnerApplicationDto> applicationDto = new Collection<ShelterOwnerApplicationDto>();

            IEnumerable<ShelterOwnerApplicationDocument> applications =
                await GetShelterSpecificApplications(query.ShelterId);

            await ConvertDocumentsToDto(applications, applicationDto);

            return applicationDto;
        }

        private async Task ConvertDocumentsToDto(IEnumerable<ShelterOwnerApplicationDocument> applications,
            ICollection<ShelterOwnerApplicationDto> applicationDto)
        {
            foreach (ShelterOwnerApplicationDocument application in applications)
            {
                ShelterDocument shelter = await GetShelterAsync(application);
                UserDto userSearch = await GetUserAsync(application);

                applicationDto.Add(application.AsDto(shelter.AsDto(), userSearch));
            }
        }

        private async Task<UserDto> GetUserAsync(ShelterOwnerApplicationDocument application)
        {
            GetResponse<UserDto> userSearch = await _elasticClient.GetAsync<UserDto>(application.UserId,
                q => q.Index(_elasticSearchOptions.Aliases.Users));

            UserDto user = userSearch?.Source;
            if (user is null)
            {
                throw new UserNotFoundException(application.UserId.ToString());
            }

            return user;
        }

        private async Task<ShelterDocument> GetShelterAsync(ShelterOwnerApplicationDocument application)
        {
            GetResponse<ShelterDocument> shelterSearch =
                await _elasticClient.GetAsync<ShelterDocument>(application.ShelterId,
                    q => q.Index(_elasticSearchOptions.Aliases.Shelters));

            ShelterDocument shelter = shelterSearch?.Source;
            if (shelter is null)
            {
                throw new ShelterNotFoundException(application.ShelterId.ToString());
            }

            return shelter;
        }

        private async Task<IEnumerable<ShelterOwnerApplicationDocument>> GetShelterSpecificApplications(Guid shelterId)
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.ShelterOwnerApplications)
            {
                Query = new BoolQuery
                {
                    Must = new List<QueryContainer>
                    {
                        new QueryContainer(new MatchPhraseQuery
                        {
                            Query = shelterId.ToString(),
                            Field = Infer.Field<ShelterOwnerApplicationDocument>(f => f.ShelterId)
                        }),
                        new QueryContainer(new TermQuery
                        {
                            Value = OwnerApplicationStatus.Pending,
                            Field = Infer.Field<ShelterOwnerApplicationDocument>(p => p.Status)
                        }),
                    }
                }
            };

            ISearchResponse<ShelterOwnerApplicationDocument>
                shelters = await _elasticClient.SearchAsync<ShelterOwnerApplicationDocument>(searchRequest);

            return shelters?.Documents.AsEnumerable();
        }
    }
}