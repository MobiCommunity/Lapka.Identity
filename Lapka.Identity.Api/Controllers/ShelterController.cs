using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Commands.Shelters;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/identity/shelter")]
    public class ShelterController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ShelterController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets shelter by ID. Longitude and latitude are not required, but if passed, distance value is returned
        /// which describes length to the shelter in meters
        /// </summary>
        /// <returns>Shelter</returns>
        /// <response code="200">If shelter is successfully returned</response>
        /// <response code="404">If shelter is not found</response>
        [ProducesResponseType(typeof(ShelterDto), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, string longitude, string latitude)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetShelterElastic
            {
                Id = id,
                Longitude = longitude,
                Latitude = latitude
            }));
        }

        /// <summary>
        /// Gets all users shelters where user is in ownership role.
        /// </summary>
        /// <returns>Shelters</returns>
        /// <response code="200">If shelters is successfully returned</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(IEnumerable<ShelterBasicDto>), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Updates shelter. User has to be in admin role, or ownership of shelter.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If shelter is successfully updated</response>
        /// <response code="400">If invalid shelter properties are given</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role, or shelter ownership</response>
        /// <response code="404">If shelter is not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateShelterRequest shelter)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateShelter(id, userAuth, shelter.Name,
                shelter.PhoneNumber, shelter.Email, shelter.BankNumber));

            return NoContent();
        }

        /// <summary>
        /// Updates shelter photo. User has to be in admin role, or ownership of shelter.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If shelter is successfully updated</response>
        /// <response code="400">If invalid shelter properties are given</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role, or shelter ownership</response>
        /// <response code="404">If shelter is not found</response>
        /// <response code="500">If connection is interrupted to files microservice</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdateShelterPhotoRequest shelter)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateShelterPhoto(id, userAuth,
                shelter.Photo.AsValueObject()));

            return NoContent();
        }

        /// <summary>
        /// Creates a shelter. User has to be in admin role.
        /// </summary>
        /// <returns>URL to created shelter</returns>
        /// <response code="201">If shelter is successfully updated</response>
        /// <response code="400">If invalid shelter properties are given</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreateShelterRequest createShelterRequest)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new CreateShelter(id, userAuth, createShelterRequest.Name,
                createShelterRequest.PhoneNumber.AsValueObject(), createShelterRequest.Email.AsValueObject(),
                createShelterRequest.Address.AsValueObject(), createShelterRequest.GeoLocation.AsValueObject(),
                createShelterRequest.Photo.AsValueObject(), createShelterRequest.BankNumber.AsValueObject()));

            return Created($"api/shelter/{id}", null);
        }

        /// <summary>
        /// Deletes a shelter. User has to be in admin role.
        /// </summary>
        /// <returns>URL to created shelter</returns>
        /// <response code="204">If shelter is successfully deleted</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role</response>
        /// <response code="404">If shelter is not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Removes user from the shelter ownership. User has to be in admin role.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If user is successfully deleted from ownership</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role</response>
        /// <response code="404">If shelter is not found or user is not owner of shelter</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("{shelterId:guid}/owners/{userId:guid}/Remove")]
        public async Task<IActionResult> Remove(Guid userId, Guid shelterId)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (userRole != "admin")
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new RemoveUserFromShelterOwners(userId, shelterId));

            return NoContent();
        }

        /// <summary>
        /// Gets all shelters. Longitude and latitude are not required, but if passed, distance value is returned
        /// which describes length to the shelter in meters.
        /// </summary>
        /// <returns>Shelters</returns>
        /// <response code="200">If shelters is successfully returned</response>
        [ProducesResponseType(typeof(IEnumerable<ShelterDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetShelters(string longitude, string latitude)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetShelters
            {
                Longitude = longitude,
                Latitude = latitude
            }));
        }
        
        /// <summary>
        /// Gets all shelters owners.
        /// </summary>
        /// <returns>Shelters owners</returns>
        /// <response code="200">If shelters owners is successfully returned</response>
        /// <response code="401">If user is not logged or admin</response>
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}/owners")]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetSheltersOwners(Guid id)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (userRole != "admin")
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetSheltersOwners
            {
                ShelterId = id
            }));
        }
    }
}