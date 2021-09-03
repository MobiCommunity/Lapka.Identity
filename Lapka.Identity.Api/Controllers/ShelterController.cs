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
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Http;

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

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateShelterRequest shelter)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateShelter(id, shelter.Name,
                shelter.PhoneNumber, shelter.Email, shelter.Address.AsValueObject(),
                shelter.GeoLocation.AsValueObject(), shelter.BankNumber));

            return NoContent();
        }

        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdateShelterPhotoRequest shelter)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateShelterPhoto(id, shelter.Photo.AsValueObject(photoId)));

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateShelterRequest createShelterRequest)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateShelter(id, createShelterRequest.Name,
                createShelterRequest.PhoneNumber, createShelterRequest.Email,
                createShelterRequest.Address.AsValueObject(),
                createShelterRequest.GeoLocation.AsValueObject(), createShelterRequest.Photo.AsValueObject(photoId),
                createShelterRequest.BankNumber));

            return Created($"api/shelter/{id}", null);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (string.IsNullOrEmpty(userRole) || userRole != "shelter")
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new DeleteShelter(id));

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetShelters()
            => Ok(await _queryDispatcher.QueryAsync(new GetShelters()));
    }
}