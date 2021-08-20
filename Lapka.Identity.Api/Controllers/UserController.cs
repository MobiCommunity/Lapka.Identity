using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IIdentityService _identityService;

        public UserController(ICommandDispatcher commandDispatcher, IIdentityService identityService)
        {
            _commandDispatcher = commandDispatcher;
            _identityService = identityService;
        }
        
        [HttpPost("SignUp")]
        public async Task<IActionResult> SingUp(SignUpRequest user)
        {
            Guid id = Guid.NewGuid();
            DateTime createdAt = DateTime.Now;;
            
            await _commandDispatcher.SendAsync(new SignUp(id, user.Username, user.FirstName, user.LastName, user.Email, 
                user.Password, user.PhotoPath, user.PhotoPath, createdAt, user.Role));

            return Created($"api/user/{id}", null);
        }
        
        [HttpPost("SignIn")]
        public async Task<IActionResult> SingIn(SignInRequest user)
        {
            AuthDto token = await _identityService.SignInAsync(new SignIn(user.Email, user.Password));

            return Ok(token);
        }
    }
}