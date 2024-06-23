using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetExpensesByYearAndMonthQuery : IRequest<List<IncomeExpensesDto>>
    {
        public GetExpensesByYearAndMonthQuery(int year, int month, int accountId)
        {
            Year = year;
            Month = month;
            AccountId = accountId;
        }
        [Required(ErrorMessage = "Year is required")]
        public int Year { get; }
        [Required(ErrorMessage = "Month is required")]
        public int Month { get; }
        [Required(ErrorMessage = "Account ID is required")]
        public int AccountId { get; }
    }
}
