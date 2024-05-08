using FinTrack.Application.Categories.Commands;
using FinTrack.Application.Categories.Queries;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            return await _mediator.Send(new GetCategoryByIdQuery(id));
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            return await _mediator.Send(new GetAllCategoriesQuery());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryDto = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}
