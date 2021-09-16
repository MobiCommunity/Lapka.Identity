using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Api.Models.Request.Auth;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Auths;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IIdentityService _identityService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthenticationController(ICommandDispatcher commandDispatcher, IRefreshTokenService refreshTokenService,
            IIdentityService identityService)
        {
            _commandDispatcher = commandDispatcher;
            _refreshTokenService = refreshTokenService;
            _identityService = identityService;
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest refreshToken)
        {
            await _commandDispatcher.SendAsync(new RevokeRefreshToken(refreshToken.Token));

            return NoContent();
        }

        [HttpPost("use")]
        public async Task<IActionResult> RevokeToken(RefreshTokenRequest refreshToken)
        {
            AuthDto token = await _refreshTokenService.UseAsync(refreshToken.Token);

            return Ok(token);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SingIn(SignInRequest user)
        {
            AuthDto token = await _identityService.SignInAsync(new SignIn(user.Email, user.Password));

            return Ok(token);
        }

        [HttpPost("signin-google")]
        public async Task<IActionResult> SingInByGoogle(SignInGoogleRequest token)
        {
            AuthDto authDto = await _identityService.SignInByGoogleAsync(new SignInGoogle(token.AccessToken));

            return Ok(authDto);
        }

        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFacebook(SignInFacebookRequest signInFacebookRequest)
        {
            AuthDto token =
                await _identityService.FacebookLoginAsync(new SignInFacebook(signInFacebookRequest.AccessToken));

            return Ok(token);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SingUp(SignUpRequest user)
        {
            Guid id = Guid.NewGuid();
            DateTime createdAt = DateTime.UtcNow;
            const string basicUserRole = "user";

            await _identityService.SignUpAsync(new SignUp(id, user.Username, user.FirstName, user.LastName, user.Email,
                user.Password, createdAt, basicUserRole));

            return Created($"api/user/{id}", null);
        }
    }
}