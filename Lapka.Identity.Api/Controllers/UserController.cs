using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Queries;

using Lapka.Identity.Infrastructure;

using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public UserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id) => Ok(await _queryDispatcher.QueryAsync(new GetUser
        {
            Id = id
        }));

        /// <summary>
        /// At the moment for testing purpose to get user's IDs
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUser() => Ok(await _queryDispatcher.QueryAsync(new GetUsers()));


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _commandDispatcher.SendAsync(new DeleteUser(id));

            return NoContent();
        }
        
        [HttpPatch("photo")]
        public async Task<IActionResult> UpdatePhoto([FromForm] UpdateUserPhotoRequest photoRequest)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateUserPhoto(userId, photoRequest.Photo.AsValueObject(photoId)));

            return NoContent();
        }
        
        [HttpPatch("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateUserPassword(userId, request.Password));

            return NoContent();
        }
        
        [HttpPatch("email")]
        public async Task<IActionResult> UpdateEmail([FromForm] UpdateUserEmailRequest photoRequest)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateUserEmail(userId, photoRequest.Email));

            return NoContent();
        }
        
        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequest user)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateUser(userId, user.Username, user.FirstName, user.LastName,
                user.PhoneNumber));

            return NoContent();
        }
        
        /// <summary>
        /// For testing purpose, at the moment to give user a shelter
        /// role, use this endpoint. 
        /// </summary>
        [HttpPatch("{userId:guid}/shelterRole")]
        public async Task<IActionResult> GiveShelterRole(Guid userId)
        {
            await _commandDispatcher.SendAsync(new GiveShelterRole(userId));

            return NoContent();
        }
    }
}