using FinTrack.Application.Auth.Commands;
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
        [HttpPost("login")]
        public async Task<IActionResult> Login(SignInCommand signInCommand)
        {
            var response = await _mediator.Send(signInCommand);
            if(response == null) 
            { 
                return Unauthorized();  
            }
            return Ok(response);      
        }
    }
}
