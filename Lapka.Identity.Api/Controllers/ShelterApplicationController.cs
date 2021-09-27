using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Shelters;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/identity/application")]
    public class ShelterApplicationController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ShelterApplicationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        
        /// <summary>
        /// Makes application for shelter ownership. User has to be logged, cannot be shelter owner and have
        /// made application for this shelter already.
        /// </summary>
        /// <returns>Created status with application ID</returns>
        /// <response code="201">If application is created, returns created status with application ID</response>
        /// <response code="400">If user is owner of shelter or already have made application for this shelter</response>
        /// <response code="401">If user was not logged</response>
        /// <response code="404">If shelter was not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add(AddApplicationRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (Guid.Empty == userId)
            {
                return Unauthorized();
            }

            Guid id = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new AddShelterOwnerApplication(id, userId, request.ShelterId));

            return Created($"api/shelter/{id}", null);
        }
        
        /// <summary>
        /// Accepts user application for shelter ownership. User has to be logged and in admin role.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If user application is successfully accepted</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role</response>
        /// <response code="404">If application or shelter was not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("{id:guid}/Accept")]
        public async Task<IActionResult> AcceptApplication(Guid id)
        {
            UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userRole.UserId == Guid.Empty)
            {
                return Unauthorized();
            }
            if (userRole.Role != "admin")
            {
                return Forbid();
            }
            
            await _commandDispatcher.SendAsync(new AcceptShelterOwnerApplication(id));

            return NoContent();
        }
        
        /// <summary>
        /// Declines user application for shelter ownership. User has to be logged and in admin role.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If user application is declined</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role</response>
        /// <response code="404">If application or shelter was not found</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [HttpPatch("{id:Guid}/Decline")]
        public async Task<IActionResult> DeclineApplication(Guid id)
        {
            UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userRole.UserId == Guid.Empty)
            {
                return Unauthorized();
            }
            if (userRole.Role != "admin")
            {
                return Forbid();
            }
            
            await _commandDispatcher.SendAsync(new DeclineShelterOwnerApplication(id));

            return NoContent();
        }

        /// <summary>
        /// Gets all users application for shelter ownership. User has to be logged and in admin role.
        /// </summary>
        /// <returns>Applications for shelter ownership</returns>
        /// <response code="200">If applications are successfully returned</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role</response>
        [ProducesResponseType(typeof(IEnumerable<ShelterDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetApplications()
        {
            UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userRole.UserId == Guid.Empty)
            {
                return Unauthorized();
            }
            if (userRole.Role != "admin")
            {
                return Forbid();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnerApplications()));
        }
        
        /// <summary>
        /// Gets applications for owner for specific shelter. User has to be logged and in admin role.
        /// </summary>
        /// <returns>Applications for shelter ownership</returns>
        /// <response code="200">If applications are successfully returned</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="403">If user is not in admin role</response>
        [ProducesResponseType(typeof(IEnumerable<ShelterDto>), StatusCodes.Status200OK)]
        [HttpGet("shelter/{id:guid}")]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetShelterApplications(Guid id)
        {
            UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userRole.UserId == Guid.Empty)
            {
                return Unauthorized();
            }
            if (userRole.Role != "admin")
            {
                return Forbid();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetSpecificShelterOwnerApplications
            {
                ShelterId = id
            }));
        }
    }
}