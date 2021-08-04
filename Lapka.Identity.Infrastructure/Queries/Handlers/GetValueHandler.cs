using Convey.CQRS.Queries;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Infrastructure.Queries.Handlers
{
    public class GetValueHandler : IQueryHandler<GetValue, ValueDto>
    {
        private readonly IValueRepository _service;

        public GetValueHandler(IValueRepository service)
        {
            _service = service;
        }

        public async Task<ValueDto> HandleAsync(GetValue query)
        {
            Value value = await _service.GetById(query.Id);

            return value.AsDto();
        }
    }
}
