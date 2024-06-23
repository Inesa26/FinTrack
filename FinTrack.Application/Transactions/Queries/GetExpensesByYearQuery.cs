using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetExpensesByYearQuery : IRequest<List<IncomeExpensesDto>>
    {
        public GetExpensesByYearQuery(int year, int accountId)
        {
            Year = year;
            AccountId = accountId;
        }
        [Required(ErrorMessage = "Year is required")]
        public int Year { get; }
        [Required(ErrorMessage = "Account ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "AccountId must be greater than 0")]
        public int AccountId { get; }
    }
}
