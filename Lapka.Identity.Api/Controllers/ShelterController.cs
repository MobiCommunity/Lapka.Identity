using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/shelter")]
    public class ShelterController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ShelterController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetShelter
            {
                Id = id
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateShelterRequest createShelterRequest)
        {
            Guid id = Guid.NewGuid();
            await _commandDispatcher.SendAsync(new CreateShelter(id, createShelterRequest.Name,
                createShelterRequest.PhoneNumber, createShelterRequest.Email,
                createShelterRequest.Address.AsValueObject(),
                createShelterRequest.GeoLocation.AsValueObject()));

            return Created($"api/shelter/{id}", null);
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _commandDispatcher.SendAsync(new DeleteShelter(id));

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetShelters() 
            => Ok(await _queryDispatcher.QueryAsync(new GetShelters()));
        
    }
}