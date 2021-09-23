using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Api.Models.Request.User;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Users;
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
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

        
        /// <summary>
        /// Gets user by ID..
        /// </summary>
        /// <returns>User</returns>
        /// <response code="200">If successfully got user</response>
        /// <response code="404">If user is not found</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetUser
            {
                Id = id
            }));
        }

        /// <summary>
        /// Gets all users. At the moment for testing purpose.
        /// </summary>
        /// <returns>Users</returns>
        /// <response code="200">If successfully got users</response>
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetUsers()));
        }

        /// <summary>
        /// Updates user photo.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated user photo</response>
        /// <response code="401">If user is not logged</response>
        /// <response code="500">If connection to files microservices was interrupted</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [HttpPatch("photo")]
        public async Task<IActionResult> UpdatePhoto([FromForm] UpdateUserPhotoRequest photoRequest)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();
            
            await _commandDispatcher.SendAsync(new UpdateUserPhoto(userId, photoRequest.Photo.AsValueObject()));

            return NoContent();
        }

        /// <summary>
        /// Updates user password.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated user password</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [HttpPatch("password")]
        public async Task<IActionResult> UpdatePassword(UpdateUserPasswordRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();

            await _commandDispatcher.SendAsync(new UpdateUserPassword(userId, request.Password));

            return NoContent();
        }

        /// <summary>
        /// Updates user email.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated user email</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [HttpPatch("email")]
        public async Task<IActionResult> UpdateEmail(UpdateUserEmailRequest photoRequest)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateUserEmail(userId, photoRequest.Email.AsValueObject()));

            return NoContent();
        }

        /// <summary>
        /// Updates user basic information.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">If successfully updated user information</response>
        /// <response code="401">If user is not logged</response>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [HttpPatch]
        public async Task<IActionResult> Update(UpdateUserRequest user)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();

            await _commandDispatcher.SendAsync(new UpdateUser(userId, user.Username, user.FirstName, user.LastName,
                user.PhoneNumber.AsValueObject()));

            return NoContent();
        }
    }
}