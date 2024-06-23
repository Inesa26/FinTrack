using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetExpensesByTransactionTypeHandler : IRequestHandler<GetExpensesByYearAndMonthQuery, List<IncomeExpensesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetExpensesByTransactionTypeHandler> _logger;

        public GetExpensesByTransactionTypeHandler(IUnitOfWork unitOfWork, ILogger<GetExpensesByTransactionTypeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<IncomeExpensesDto>> Handle(GetExpensesByYearAndMonthQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var startDate = new DateTime(request.Year, request.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var transactions = await _unitOfWork.TransactionRepository
                    .Filter(q => q.Where(t =>
                        t.Date >= startDate &&
                        t.Date <= endDate &&
                        t.Type == TransactionType.Expense &&
                        t.AccountId == request.AccountId)); // Filter by AccountId

                // Group transactions by category and sum the amounts
                var expenseGroupedByCategory = transactions
                    .GroupBy(t => t.CategoryId)
                    .Select(g => new
                    {
                        CategoryId = g.Key,
                        TotalAmount = g.Sum(t => t.Amount)
                    });

                var expenseDtos = new List<IncomeExpensesDto>();
                foreach (var group in expenseGroupedByCategory)
                {
                    var category = await _unitOfWork.CategoryRepository.Get(group.CategoryId);
                    if (category != null)
                    {
                        var expenseDto = new IncomeExpensesDto
                        {
                            Amount = group.TotalAmount,
                            CategoryName = category.Title
                        };
                        expenseDtos.Add(expenseDto);
                    }
                }

                _logger.LogInformation("Expenses for year {Year} and month {Month} retrieved successfully.", request.Year, request.Month);
                return expenseDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve expenses for year {Year} and month {Month}.", request.Year, request.Month);
                throw;
            }
        }
    }
}
