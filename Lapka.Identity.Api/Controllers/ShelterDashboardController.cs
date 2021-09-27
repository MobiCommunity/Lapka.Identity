using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.Dashboards;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/identity/shelter/{id:guid}/dashboard")]
    public class ShelterDashboardController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public ShelterDashboardController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }
        
        
        /// <summary>
        /// Gets shelter total views. User has to be logged.
        /// </summary>
        [ProducesResponseType(typeof(ShelterViewsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpGet("view/count")]
        public async Task<IActionResult> GetShelterViews(Guid id)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetShelterViewsCount
            {
                ShelterId = id
            }));
        }

        /// <summary>
        /// Increases total views of shelter. User has to be logged.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPatch("view/count")]
        public async Task<IActionResult> IncrementShelterViews(Guid id)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new IncrementShelterViews(id));

            return NoContent();
        }
        
        /// <summary>
        /// Gets total number of applications for this shelter. User has to be logged and in admin role.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
        [HttpGet("application/count")]
        public async Task<IActionResult> GetApplicationCount(Guid id)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }

            if (userAuth.Role != "admin")
            {
                return Forbid();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnerApplicationsCount
            {
                ShelterId = id
            }));
        }
    }
}