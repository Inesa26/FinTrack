using FinTrack.Application.Categories.Commands;
using FinTrack.Application.Categories.Queries;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/category")]
   
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {

            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryIconDto>>> GetAllCategories(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] TransactionType transactionType = 0)

        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(pageIndex, pageSize, transactionType));
            return Ok(result);
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
