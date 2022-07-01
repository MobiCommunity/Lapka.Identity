// using Convey.CQRS.Commands;
// using Convey.CQRS.Queries;
// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Lapka.Identity.Api.Models;
// using Lapka.Identity.Api.Models.Request;
// using Lapka.Identity.Application.Commands.ShelterOwnership;
// using Lapka.Identity.Application.Dto;
// using Lapka.Identity.Application.Dto.Shelters;
// using Lapka.Identity.Application.Queries;
// using Lapka.Identity.Application.Queries.Shelters;
// using Lapka.Identity.Core.ValueObjects;
// using Lapka.Identity.Infrastructure;
// using Microsoft.AspNetCore.Http;
//
// namespace Lapka.Identity.Api.Controllers
// {
//     [ApiController]
//     [Route("api/identity/application")]
//     public class ShelterApplicationController : ControllerBase
//     {
//         private readonly ICommandDispatcher _commandDispatcher;
//         private readonly IQueryDispatcher _queryDispatcher;
//
//         public ShelterApplicationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
//         {
//             _commandDispatcher = commandDispatcher;
//             _queryDispatcher = queryDispatcher;
//         }
//         
//         
//         /// <summary>
//         /// Makes application for shelter ownership. User has to be logged, cannot be shelter owner and have
//         /// made application for this shelter already.
//         /// </summary>
//         [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
//         [HttpPost]
//         public async Task<IActionResult> Add(AddApplicationRequest request)
//         {
//             Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
//             if (Guid.Empty == userId)
//             {
//                 return Unauthorized();
//             }
//
//             Guid id = Guid.NewGuid();
//
//             await _commandDispatcher.SendAsync(new AddShelterOwnerApplication(id, userId, request.ShelterId));
//
//             return Created($"api/shelter/{id}", null);
//         }
//         
//         /// <summary>
//         /// Accepts user application for shelter ownership. User has to be logged and in admin role.
//         /// </summary>
//         [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
//         [HttpPatch("{id:guid}/Accept")]
//         public async Task<IActionResult> AcceptApplication(Guid id)
//         {
//             UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
//             if (userRole.UserId == Guid.Empty)
//             {
//                 return Unauthorized();
//             }
//             if (userRole.Role != UserRoles.Admin.ToString())
//             {
//                 return Forbid();
//             }
//             
//             await _commandDispatcher.SendAsync(new AcceptShelterOwnerApplication(id));
//
//             return NoContent();
//         }
//         
//         /// <summary>
//         /// Declines user application for shelter ownership. User has to be logged and in admin role.
//         /// </summary>
//         [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
//         [HttpPatch("{id:Guid}/Decline")]
//         public async Task<IActionResult> DeclineApplication(Guid id)
//         {
//             UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
//             if (userRole.UserId == Guid.Empty)
//             {
//                 return Unauthorized();
//             }
//             if (userRole.Role != UserRoles.Admin.ToString())
//             {
//                 return Forbid();
//             }
//             
//             await _commandDispatcher.SendAsync(new DeclineShelterOwnerApplication(id));
//
//             return NoContent();
//         }
//
//         /// <summary>
//         /// Gets all users application for shelter ownership. User has to be logged and in admin role.
//         /// </summary>
//         [ProducesResponseType(typeof(IEnumerable<ShelterOwnerApplicationDto>), StatusCodes.Status200OK)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<ShelterOwnerApplicationDto>>> GetApplications()
//         {
//             UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
//             if (userRole.UserId == Guid.Empty)
//             {
//                 return Unauthorized();
//             }
//             if (userRole.Role != UserRoles.Admin.ToString())
//             {
//                 return Forbid();
//             }
//             
//             return Ok(await _queryDispatcher.QueryAsync(new GetShelterOwnerApplications()));
//         }
//         
//         /// <summary>
//         /// Gets applications for owner for specific shelter. User has to be logged and in admin role.
//         /// </summary>
//         [ProducesResponseType(typeof(IEnumerable<ShelterOwnerApplicationDto>), StatusCodes.Status200OK)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
//         [ProducesResponseType(typeof(object), StatusCodes.Status403Forbidden)]
//         [HttpGet("shelter/{id:guid}")]
//         public async Task<ActionResult<IEnumerable<ShelterOwnerApplicationDto>>> GetShelterApplications(Guid id)
//         {
//             UserAuth userRole = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
//             if (userRole.UserId == Guid.Empty)
//             {
//                 return Unauthorized();
//             }
//             if (userRole.Role != UserRoles.Admin.ToString())
//             {
//                 return Forbid();
//             }
//             
//             return Ok(await _queryDispatcher.QueryAsync(new GetSpecificShelterOwnerApplications
//             {
//                 ShelterId = id
//             }));
//         }
//     }
// }