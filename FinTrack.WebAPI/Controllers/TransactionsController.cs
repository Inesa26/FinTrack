using FinTrack.Application.Responses;
using FinTrack.Application.Transactions.Commands;
using FinTrack.Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ILogger<TransactionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById(int id)
        {
            return await _mediator.Send(new GetTransactionByIdQuery(id));
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAllTransactions()
        {
            return await _mediator.Send(new GetAllTransactionsQuery());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var transactionDto = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetTransactionById), new { id = transactionDto.Id }, transactionDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransaction([FromBody] UpdateTransactionCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _mediator.Send(new DeleteTransactionCommand(id));
            return NoContent();
        }
    }
}
