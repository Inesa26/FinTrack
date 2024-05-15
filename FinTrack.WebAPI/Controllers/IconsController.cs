using FinTrack.Application.Icons.Commands;
using FinTrack.Application.Icons.Queries;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebAPI.Controllers
{
    [Route("api/icon")]
    [ApiController]
    [Authorize]
    public class IconsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<IconsController> _logger;

        public IconsController(ILogger<IconsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IconDto>> GetIconById(int id)
        {
            return await _mediator.Send(new GetIconByIdQuery(id));
        }

        [HttpGet]
        public async Task<ActionResult<List<IconDto>>> GetAllIcons()
        {
            return await _mediator.Send(new GetAllIconsQuery());
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
