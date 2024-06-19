using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/monthly-summary")]
    [Authorize]
    public class MonthlySummaryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMonthlySummaryService _monthlySummaryService;

        public MonthlySummaryController(IUnitOfWork unitOfWork, IMonthlySummaryService monthlySummaryService)
        {
            _unitOfWork = unitOfWork;
            _monthlySummaryService = monthlySummaryService;
        }

        [HttpGet]
        public async Task<ActionResult<MonthlySummaryDto>> GetMonthlySummaryByYearAndMonth(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var account = await _unitOfWork.AccountRepository.GetSingle(q => q.Where(a => a.UserId == userId));
            if (account == null)
                return NotFound("Account not found for the user");

            var result = await _monthlySummaryService.GetMonthlySummary(account.Id, year, (Month)month);

            return Ok(result);
        }

        [HttpGet("year")]
        public async Task<ActionResult<List<MonthlySummaryDto>>> GetMonthlySummariesPerYear(
            [FromQuery] int year)
        {
            var monthlyUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (monthlyUserId == null)
                return Unauthorized();

            var monthlyAccount = await _unitOfWork.AccountRepository.GetSingle(q => q.Where(a => a.UserId == monthlyUserId));
            if (monthlyAccount == null)
                return NotFound("Account not found for the user");

            var summaryResult = await _monthlySummaryService.GetMonthlySummariesForYear(monthlyAccount.Id, year);

            if (summaryResult == null)
                return NotFound($"No monthly summaries found for the year {year}.");

            return Ok(summaryResult);
        }
    }
}
