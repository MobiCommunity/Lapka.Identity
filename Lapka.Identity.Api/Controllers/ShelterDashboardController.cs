using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Commands.Dashboards;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure;
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