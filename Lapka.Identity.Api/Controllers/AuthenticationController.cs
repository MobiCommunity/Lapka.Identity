using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Api.Models.Request;
using Lapka.Identity.Api.Models.Request.Auth;
using Lapka.Identity.Application.Commands;
using Lapka.Identity.Application.Commands.Auth;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Dto.Auths;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Application.Services.Auth;
using Lapka.Identity.Core.Exceptions.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/identity/auth")]
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


        /// <summary>
        /// Revokes refresh token.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <response code="204">If token is successfully revoked</response>
        /// <response code="400">If the token is not found</response>
        [ProducesResponseType(typeof(object), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest refreshToken)
        {
            await _commandDispatcher.SendAsync(new RevokeRefreshToken(refreshToken.Token));

            return NoContent();
        }

        /// <summary>
        /// Revokes refresh token.
        /// </summary>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [HttpPost("use")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshToken)
        {
            AuthDto token = await _refreshTokenService.UseAsync(refreshToken.Token);

            return Ok(token);
        }

        /// <summary>
        /// Signs in to the app. For testing purpose login by admin is enabled, by providing
        /// credentials { "email": "admin@admin.com", "password": "admin" }
        /// </summary>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [HttpPost("signin")]
        public async Task<IActionResult> SingIn(SignInRequest user)
        {
            AuthDto token = await _identityService.SignInAsync(new SignIn(user.Email, user.Password));

            return Ok(token);
        }

        /// <summary>
        /// Signs in to the app using google account.
        /// </summary>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [HttpPost("signin-google")]
        public async Task<IActionResult> SingInByGoogle(SignInGoogleRequest token)
        {
            AuthDto authDto = await _identityService.SignInByGoogleAsync(new SignInGoogle(token.AccessToken));

            return Ok(authDto);
        }

        /// <summary>
        /// Signs in to the app using facebook account.
        /// </summary>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [HttpPost("signin-facebook")]
        public async Task<IActionResult> SignInFacebook(SignInFacebookRequest signInFacebookRequest)
        {
            AuthDto token =
                await _identityService.FacebookLoginAsync(new SignInFacebook(signInFacebookRequest.AccessToken));

            return Ok(token);
        }

        /// <summary>
        /// Signs up to the app.
        /// </summary>
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [HttpPost("signup")]
        public async Task<IActionResult> SingUp(SignUpRequest user)
        {
            Guid id = Guid.NewGuid();
            DateTime createdAt = DateTime.UtcNow;
            const string basicUserRole = "user";

            await _identityService.SignUpAsync(new SignUp(id, user.Username, user.FirstName, user.LastName,
                user.Email, user.Password, createdAt, basicUserRole));

            return Created($"api/user/{id}", null);
        }
    }
}