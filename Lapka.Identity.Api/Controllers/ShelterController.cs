using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetById(Guid id, string longitude, string latitude)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetShelter
            {
                Id = id,
                Longitude = longitude,
                Latitude = latitude
            }));
        }
        
        [HttpGet("user")]
        public async Task<IActionResult> GetUserShelters()
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetUserShelters
            {
                UserId = userId
            }));
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateShelterRequest shelter)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateShelter(id, userAuth, shelter.Name,
                shelter.PhoneNumber, shelter.Email, shelter.Address.AsValueObject(),
                shelter.GeoLocation.AsValueObject(), shelter.BankNumber));

            return NoContent();
        }

        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdateShelterPhotoRequest shelter)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }

            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateShelterPhoto(id, userAuth,
                shelter.Photo.AsValueObject(photoId)));

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateShelterRequest createShelterRequest)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateShelter(id, userAuth, createShelterRequest.Name,
                createShelterRequest.PhoneNumber, createShelterRequest.Email,
                createShelterRequest.Address.AsValueObject(),
                createShelterRequest.GeoLocation.AsValueObject(), createShelterRequest.Photo.AsValueObject(photoId),
                createShelterRequest.BankNumber));

            return Created($"api/shelter/{id}", null);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new DeleteShelter(id, userAuth));

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetShelters(string longitude, string latitude)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetShelters
            {
                Longitude = longitude,
                Latitude = latitude
            }));
        }
    }
}