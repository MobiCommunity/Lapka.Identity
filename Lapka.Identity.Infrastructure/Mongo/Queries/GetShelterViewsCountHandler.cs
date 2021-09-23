using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions.Shelters;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Application.Services.Repositories;
using Lapka.Identity.Core.Entities;
using Lapka.Identity.Core.ValueObjects;
using Lapka.Identity.Infrastructure.Mongo.Documents;

namespace Lapka.Identity.Infrastructure.Mongo.Queries
{
    public class GetShelterViewsCountHandler : IQueryHandler<GetShelterViewsCount, ShelterViewsDto>
    {
        private readonly IMongoRepository<ShelterViewsDocument, Guid> _shelterViewsRepository;
        private readonly IMongoRepository<ShelterDocument, Guid> _shelterRepository;

        public GetShelterViewsCountHandler(IMongoRepository<ShelterViewsDocument, Guid> shelterViewsRepository,
            IMongoRepository<ShelterDocument, Guid> shelterRepository)
        {
            _shelterViewsRepository = shelterViewsRepository;
            _shelterRepository = shelterRepository;
        }
        
        public async Task<ShelterViewsDto> HandleAsync(GetShelterViewsCount query)
        {
            ShelterViewsDocument shelterViews = await _shelterViewsRepository.GetAsync(query.ShelterId);
            if (shelterViews is null)
            {
                ShelterDocument shelter = await _shelterRepository.GetAsync(query.ShelterId);
                if (shelter is null)
                {
                    throw new ShelterNotFoundException(shelter.Id.ToString());
                }

                await _shelterViewsRepository.AddAsync(new ShelterViewsDocument
                {
                    Id = shelter.Id,
                    ActualMonthViewsCount = 0,
                    PreviousMonthsViews = new List<ViewHistoryDocument>()
                });
                
                shelterViews = await _shelterViewsRepository.GetAsync(query.ShelterId);
            }

            return shelterViews.AsDto();
        }
    }
}