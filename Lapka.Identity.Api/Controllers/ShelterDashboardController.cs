using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.Dashboards;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/shelter/{id:guid}/dashboard")]
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
        /// <returns>Shelter views</returns>
        /// <response code="200">If successfully got shelter views</response>
        /// <response code="404">If shelter is not found</response>
        [ProducesResponseType(typeof(ShelterViewsDto), StatusCodes.Status200OK)]
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
        /// <returns>No content</returns>
        /// <response code="204">If successfully increased views</response>
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
    }
}