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


        /// <summary>
        /// Revokes refresh token.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <response code="204">If token is successfully revoked</response>
        /// <response code="400">If the token is not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest refreshToken)
        {
            await _commandDispatcher.SendAsync(new RevokeRefreshToken(refreshToken.Token));

            return NoContent();
        }

        /// <summary>
        /// Revokes refresh token.
        /// </summary>
        /// <returns>A newly created refresh token</returns>
        /// <response code="200">If token is successfully refreshed</response>
        /// <response code="400">If token is not found or invalid</response>
        /// <response code="404">If user is not found</response>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
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
        /// <returns>Refresh tokens</returns>
        /// <response code="200">If user is successfully logged, returns tokens</response>
        /// <response code="400">If user is not found, or password is incorrect</response>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [HttpPost("signin")]
        public async Task<IActionResult> SingIn(SignInRequest user)
        {
            AuthDto token = await _identityService.SignInAsync(new SignIn(user.Email, user.Password));

            return Ok(token);
        }

        /// <summary>
        /// Signs in to the app using google account.
        /// </summary>
        /// <returns>Refresh tokens</returns>
        /// <response code="200">If user is successfully logged, returns tokens</response>
        /// <response code="400">If token is invalid</response>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [HttpPost("signin-google")]
        public async Task<IActionResult> SingInByGoogle(SignInGoogleRequest token)
        {
            AuthDto authDto = await _identityService.SignInByGoogleAsync(new SignInGoogle(token.AccessToken));

            return Ok(authDto);
        }

        /// <summary>
        /// Signs in to the app using facebook account.
        /// </summary>
        /// <returns>Refresh tokens</returns>
        /// <response code="200">If user is successfully logged, returns tokens</response>
        /// <response code="400">If token is invalid</response>
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
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
        /// <returns>Created status with user ID</returns>
        /// <response code="201">If user is successfully logged, returns created response with user id</response>
        /// <response code="400">If user email is taken, or password is too short</response>
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [HttpPost("signup")]
        public async Task<IActionResult> SingUp(SignUpRequest user)
        {
            Guid id = Guid.NewGuid();
            DateTime createdAt = DateTime.UtcNow;
            const string basicUserRole = "user";

            await _identityService.SignUpAsync(new SignUp(id, user.Username, user.FirstName, user.LastName,
                user.Email.AsValueObject(), user.Password, createdAt, basicUserRole));

            return Created($"api/user/{id}", null);
        }
    }
}