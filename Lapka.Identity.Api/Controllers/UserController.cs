using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UserController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        [HttpPost("SignUp")]
        public async Task<IActionResult> SingUp([FromForm]SignUpRequest user)
        {
            Guid id = Guid.NewGuid();
            DateTime createdAt = DateTime.Now;
            
            await _identityService.SignUpAsync(new SignUp(id, user.Username, user.FirstName, user.LastName, user.Email, 
                user.Password, createdAt));

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