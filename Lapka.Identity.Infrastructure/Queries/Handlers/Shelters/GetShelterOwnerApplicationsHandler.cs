using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure.Documents;

namespace Lapka.Identity.Infrastructure.Queries.Handlers.Shelters
{
    public class GetShelterOwnerApplicationsHandler : IQueryHandler<GetShelterOwnerApplications,
        IEnumerable<ShelterOwnerApplicationDto>>
    {
        private readonly IMongoRepository<ShelterOwnerApplicationDocument, Guid> _applicationRepository;
        private readonly IMongoRepository<ShelterDocument, Guid> _shelterRepository;
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;

        public GetShelterOwnerApplicationsHandler(IMongoRepository<ShelterOwnerApplicationDocument, Guid> applicationRepository,
            IMongoRepository<ShelterDocument, Guid> shelterRepository, IMongoRepository<UserDocument, Guid> userRepository)
        {
            _applicationRepository = applicationRepository;
            _shelterRepository = shelterRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ShelterOwnerApplicationDto>> HandleAsync(GetShelterOwnerApplications query)
        {
            List<ShelterOwnerApplicationDto> applicationDto = new List<ShelterOwnerApplicationDto>();
            
            IReadOnlyList<ShelterOwnerApplicationDocument> applications = await _applicationRepository.FindAsync(_ => true);
            foreach (ShelterOwnerApplicationDocument application in applications)
            {
                ShelterDocument shelter = await _shelterRepository.GetAsync(application.ShelterId);
                UserDocument user = await _userRepository.GetAsync(application.UserId);
                applicationDto.Add(application.AsDto(shelter.AsDto(), user.AsDto()));
            }

            return applicationDto;
        }
    }
}