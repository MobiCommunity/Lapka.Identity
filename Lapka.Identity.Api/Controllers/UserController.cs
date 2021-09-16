using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Api.Models.Request.User;
using Lapka.Identity.Application.Commands.Users;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Users;
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
        public async Task<IActionResult> GetUser(Guid id)
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetUser
            {
                Id = id
            }));
        }

        /// <summary>
        /// At the moment for testing purpose to get all user's IDs
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _queryDispatcher.QueryAsync(new GetUsers()));
        }

        [HttpPatch("photo")]
        public async Task<IActionResult> UpdatePhoto([FromForm] UpdateUserPhotoRequest photoRequest)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();

            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateUserPhoto(userId, photoRequest.Photo.AsValueObject(photoId)));

            return NoContent();
        }

        [HttpPatch("password")]
        public async Task<IActionResult> UpdatePassword(UpdateUserPasswordRequest request)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();

            await _commandDispatcher.SendAsync(new UpdateUserPassword(userId, request.Password));

            return NoContent();
        }

        [HttpPatch("email")]
        public async Task<IActionResult> UpdateEmail(UpdateUserEmailRequest photoRequest)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();

            await _commandDispatcher.SendAsync(new UpdateUserEmail(userId, photoRequest.Email));

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> Update(UpdateUserRequest user)
        {
            Guid userId = await HttpContext.AuthenticateUsingJwtGetUserIdAsync();
            if (userId == Guid.Empty) return Unauthorized();

            await _commandDispatcher.SendAsync(new UpdateUser(userId, user.Username, user.FirstName, user.LastName,
                user.PhoneNumber));

            return NoContent();
        }

        /// <summary>
        /// For testing purpose, adds admin if does not exists. Login - Email: admin@admin.com | Password: admin
        /// </summary>
        [HttpPost("admin")]
        public async Task<IActionResult> AddAdmin()
        {
            await _commandDispatcher.SendAsync(new CreateAdmin());

            return NoContent();
        }
    }
}