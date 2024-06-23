using FinTrack.Application.Abstractions;
using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using FinTrack.Application.Transactions.Commands;
using FinTrack.Application.Transactions.Queries;
using FinTrack.WebAPI.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionsController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransactionById(int id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery(id));
            return Ok(result);
        }
      
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<TransactionDto>>> GetAllTransactions(
           [FromQuery] int pageIndex = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string sortBy = "Date",
           [FromQuery] string sortOrder = "asc")
        {
            var transactionUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (transactionUserId == null)
                return Unauthorized();

            var transactionAccount = await _unitOfWork.AccountRepository.GetSingle(q => q.Where(a => a.UserId == transactionUserId));
            if (transactionAccount == null)
            {
                return NotFound("Account not found for the user");
            }
            var query = new GetAllTransactionsQuery(transactionAccount.Id, pageIndex, pageSize, sortBy, sortOrder);
            var transactionResult = await _mediator.Send(query);
            return Ok(transactionResult);
        }
    

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var account = await _unitOfWork.AccountRepository.GetSingle(q => q.Where(a => a.UserId == userId));
            if (account == null)
                return NotFound("Account not found for the user");
            CreateTransactionCommand createTransactionCommand = new CreateTransactionCommand(
                account.Id, request.Amount, request.Date,
                request.Description, request.CategoryId, request.TransactionType);


            var transactionDto = await _mediator.Send(createTransactionCommand);

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

        [HttpGet("incomes")]
        public async Task<ActionResult<List<IncomeExpensesDto>>> GetIncomesByYearAndMonth([FromQuery] int year, [FromQuery] int month, [FromQuery] int accountId)
        {
            var incomesQuery = new GetIncomesByYearAndMonthQuery(year, month, accountId);
            var incomesResult = await _mediator.Send(incomesQuery);
            return Ok(incomesResult);
        }

        [HttpGet("expenses")]
        public async Task<ActionResult<List<IncomeExpensesDto>>> GetExpensesByYearAndMonth([FromQuery] int year, [FromQuery] int month, [FromQuery] int accountId)
        {
            var expenseQuery = new GetExpensesByYearAndMonthQuery(year, month, accountId);
            var expenseResult = await _mediator.Send(expenseQuery);
            return Ok(expenseResult);
        }

        [HttpGet("year-expenses")]
        public async Task<ActionResult<List<IncomeExpensesDto>>> GetCategoryExpensesByYear([FromQuery] int year, [FromQuery] int accountId)
        {
            var query = new GetExpensesByYearQuery( year, accountId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
