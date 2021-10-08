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
using Lapka.Identity.Core.ValueObjects;
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
        [ProducesResponseType(typeof(ShelterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(IEnumerable<ShelterBasicDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
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
                createShelterRequest.PhoneNumber, createShelterRequest.Email,
                createShelterRequest.Address.AsValueObject(), createShelterRequest.GeoLocation.AsValueObject(),
                createShelterRequest.Photo.AsValueObject(), createShelterRequest.BankNumber));

            return Created($"api/shelter/{id}", null);
        }

        /// <summary>
        /// Deletes a shelter. User has to be in admin role.
        /// </summary>
        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpPatch("{shelterId:guid}/owners/{userId:guid}/Remove")]
        public async Task<IActionResult> Remove(Guid userId, Guid shelterId)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (userRole != UserRoles.Admin.ToString())
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
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [HttpGet("{id:guid}/owners")]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetSheltersOwners(Guid id)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (userRole != UserRoles.Admin.ToString())
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