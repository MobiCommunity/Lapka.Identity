using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/shelter/create")]
    public class CreateShelterController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public CreateShelterController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NewShelter(CreateShelterRequest createShelterRequest)
        {
            Guid id = Guid.NewGuid();
            await _commandDispatcher.SendAsync(new CreateShelter(id, createShelterRequest.Name,
                createShelterRequest.PhoneNumber, createShelterRequest.Email, createShelterRequest.Address.AsValueObject(),
                createShelterRequest.GeoLocation.AsValueObject()));

            return Created($"api/shleter/{id}", null);
        }
    }
}