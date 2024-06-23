using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetIncomesByYearAndMonthQuery : IRequest<List<IncomeExpensesDto>>
    {
        public GetIncomesByYearAndMonthQuery(int year, int month, int accountId)
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
        [Range(1, int.MaxValue, ErrorMessage = "AccountId must be greater than 0")]
        public int AccountId { get; }
    }
}
