﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Lapka.Identity.Api.Models;
using Lapka.Identity.Application.Queries;
using Lapka.Identity.Application.Queries.Shelters;
using Lapka.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Lapka.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/shelter/{id:guid}/dashboard")]
    public class ShelterDashboardController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ShelterDashboardController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpGet("cards/count")]
        public async Task<IActionResult> GetById(Guid id)
        {
            UserAuth userAuth = await HttpContext.AuthenticateUsingJwtGetUserAuthAsync();
            if (userAuth is null)
            {
                return Unauthorized();
            }
            
            return Ok(await _queryDispatcher.QueryAsync(new GetShelterCardsCount
            {
                Auth = userAuth,
                ShelterId = id
            }));
        }
    }
}