using FinTrack.Application.Icons.Commands;
using FinTrack.Application.Icons.Queries;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebAPI.Controllers
{
    [Route("api/icon")]
    [ApiController]
    
    public class IconsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IconsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IconDto>> GetIconById(int id)
        {
            var result = await _mediator.Send(new GetIconByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<IconDto>>> GetAllIcons(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
             [FromQuery] TransactionType transactionType = 0)
        {
            var result = await _mediator.Send(new GetAllIconsQuery(pageIndex, pageSize, transactionType));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIcon([FromBody] CreateIconCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var iconDto = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetIconById), new { id = iconDto.Id }, iconDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIcon([FromBody] UpdateIconCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIcon(int id)
        {
            await _mediator.Send(new DeleteIconCommand(id));
            return NoContent();
        }
    }
}
