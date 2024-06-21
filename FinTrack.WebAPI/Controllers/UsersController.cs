using FinTrack.Application.Responses;
using FinTrack.Application.Users.Commands;
using FinTrack.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebAPI.Controllers
{
    [Authorize]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


       
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            GetUserByIdQuery query = new GetUserByIdQuery(id);
            var userData = await _mediator.Send(query);
            return Ok(userData);
        }

        [HttpPut]
        public async Task<ActionResult<UpdatedUserDto>> UpdateUser([FromBody] UpdateUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedUser = await _mediator.Send(command);
            return Ok(updatedUser);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _mediator.Send(command);

            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to update password.");
        }
    }


}
