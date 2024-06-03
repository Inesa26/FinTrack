﻿using FinTrack.Application.Accounts.Commands;
using FinTrack.Application.Auth.Commands;
using FinTrack.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebAPI.Controllers
{
    [Route("api/account")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand createUserCommand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userDto = await _mediator.Send(createUserCommand);
            if (userDto == null)
            {
                return BadRequest("Failed to create user");
            }
            var accountDto = await _mediator.Send(new CreateAccountCommand(userDto.Id));
            if (accountDto == null)
            {
                return BadRequest($"Failed to create account for UserId: {userDto.Id}");
            }
            var response = new { User = userDto, Account = accountDto };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInCommand signInCommand)
        {
            var response = await _mediator.Send(signInCommand);
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(response);
        }
    }
}
