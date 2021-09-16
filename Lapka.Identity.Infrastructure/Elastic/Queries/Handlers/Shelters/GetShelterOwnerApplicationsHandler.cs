using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Exceptions.Users;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure.Elastic.Options;
using Lapka.Identity.Infrastructure.Mongo.Documents;
using Nest;

namespace Lapka.Identity.Infrastructure.Elastic.Queries.Handlers.Shelters
{
    public class GetShelterOwnerApplicationsHandler : IQueryHandler<GetShelterOwnerApplications,
        IEnumerable<ShelterOwnerApplicationDto>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticSearchOptions _elasticSearchOptions;

        public GetShelterOwnerApplicationsHandler(IElasticClient elasticClient,
            ElasticSearchOptions elasticSearchOptions)
        {
            _elasticClient = elasticClient;
            _elasticSearchOptions = elasticSearchOptions;
        }

        public async Task<IEnumerable<ShelterOwnerApplicationDto>> HandleAsync(GetShelterOwnerApplications query)
        {
            List<ShelterOwnerApplicationDto> applicationDto = new List<ShelterOwnerApplicationDto>();
            
            List<ShelterOwnerApplicationDocument> applications = await GetAllApplications();

            await ConvertDocumentsToDto(applications, applicationDto);
            
            return applicationDto;
        }

        private async Task ConvertDocumentsToDto(List<ShelterOwnerApplicationDocument> applications, List<ShelterOwnerApplicationDto> applicationDto)
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

        private async Task<List<ShelterOwnerApplicationDocument>> GetAllApplications()
        {
            ISearchRequest searchRequest = new SearchRequest(_elasticSearchOptions.Aliases.ShelterOwnerApplications);

            ISearchResponse<ShelterOwnerApplicationDocument>
                shelters = await _elasticClient.SearchAsync<ShelterOwnerApplicationDocument>(searchRequest);
            
            return shelters?.Documents.ToList();
        }
    }
}