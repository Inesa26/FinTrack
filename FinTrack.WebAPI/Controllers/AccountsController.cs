using FinTrack.Application.Accounts.Commands;
using FinTrack.Application.Accounts.Queries;
using FinTrack.Application.Auth.Commands;
using FinTrack.Application.Responses;
using FinTrack.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Login([FromBody] LogInCommand logInCommand)
        {
            var response = await _mediator.Send(logInCommand);
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AccountDto>> GetBalance([FromQuery] int accountId)
        {
            GetBalanceQuery query = new GetBalanceQuery { AccountId = accountId };
            var balance = await _mediator.Send(query);
            return Ok(balance);
        }

    }
}
