using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands.ShelterOwnership;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Infrastructure;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/application")]
    public class ShelterApplicationController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ShelterApplicationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddApplicationRequest request)
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
        
        [HttpPatch("{id:guid}/Accept")]
        public async Task<IActionResult> AcceptApplication(Guid id)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (userRole != "admin")
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new AcceptShelterOwnerApplication(id));

            return NoContent();
        }
        
        [HttpPatch("{id:Guid}/Decline")]
        public async Task<IActionResult> DeclineApplication(Guid id)
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (userRole != "admin")
            {
                return Unauthorized();
            }
            
            await _commandDispatcher.SendAsync(new DeclineShelterOwnerApplication(id));

            return NoContent();
        }
        
        [HttpPatch("{userId:guid}/shelter/{shelterId:Guid}/Remove")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShelterDto>>> GetApplications()
        {
            string userRole = await HttpContext.AuthenticateUsingJwtGetUserRoleAsync();
            if (userRole != "admin")
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnerApplications()));
        }
    }
}