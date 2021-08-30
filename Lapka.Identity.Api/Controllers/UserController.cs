using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUser() => Ok(await _queryDispatcher.QueryAsync(new GetUsers()));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _commandDispatcher.SendAsync(new DeleteUser(id));

            return NoContent();
        }

        [HttpPatch("{id:guid}/photo")]
        public async Task<IActionResult> UpdatePhoto(Guid id, [FromForm] UpdateUserPhotoRequest photoRequest)
        {
            Guid photoId = Guid.NewGuid();

            await _commandDispatcher.SendAsync(new UpdateUserPhoto(id, photoRequest.Photo.AsValueObject(), photoId));

            return NoContent();
        }

        [HttpPatch("{id:guid}/password")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdateUserPasswordRequest request)
        {
            await _commandDispatcher.SendAsync(new UpdateUserPassword(id, request.Password));

            return NoContent();
        }

        [HttpPatch("{id:guid}/email")]
        public async Task<IActionResult> UpdateEmail(Guid id, [FromForm] UpdateUserEmailRequest photoRequest)
        {
            await _commandDispatcher.SendAsync(new UpdateUserEmail(id, photoRequest.Email));

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserRequest user)
        {
            await _commandDispatcher.SendAsync(new UpdateUser(id, user.Username, user.FirstName, user.LastName,
                user.PhoneNumber));

            return NoContent();
        }
    }
}