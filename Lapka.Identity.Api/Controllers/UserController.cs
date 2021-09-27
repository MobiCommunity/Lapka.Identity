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
    [Route("api/identity/user")]
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
        /// Gets user by ID.
        /// </summary>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetUsers()));
        }

        /// <summary>
        /// Updates user photo.
        /// </summary>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [HttpPatch("email")]
        public async Task<IActionResult> UpdateEmail(UpdateUserEmailRequest photoRequest)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            await _commandDispatcher.SendAsync(new UpdateUserEmail(userId, photoRequest.Email));

            return NoContent();
        }

        /// <summary>
        /// Updates user basic information.
        /// </summary>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [HttpPatch]
        public async Task<IActionResult> Update(UpdateUserRequest user)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();

            await _commandDispatcher.SendAsync(new UpdateUser(userId, user.Username, user.FirstName, user.LastName,
                user.PhoneNumber));

            return NoContent();
        }
    }
}