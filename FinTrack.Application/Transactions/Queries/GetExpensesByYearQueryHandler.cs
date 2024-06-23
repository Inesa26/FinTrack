using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetExpensesByYearQueryHandler : IRequestHandler<GetExpensesByYearQuery, List<IncomeExpensesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetExpensesByYearQueryHandler> _logger;

        public GetExpensesByYearQueryHandler(IUnitOfWork unitOfWork, ILogger<GetExpensesByYearQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<IncomeExpensesDto>> Handle(GetExpensesByYearQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var startDate = new DateTime(request.Year, 1, 1);
                var endDate = startDate.AddYears(1).AddDays(-1);

                var transactions = await _unitOfWork.TransactionRepository
                    .Filter(q => q.Where(t =>
                        t.Date >= startDate &&
                        t.Date <= endDate &&
                        t.Type == TransactionType.Expense &&
                        t.AccountId == request.AccountId));

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

                _logger.LogInformation("Expenses for year {Year} retrieved successfully.", request.Year);
                return expenseDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve expenses for year {Year}.", request.Year);
                throw;
            }
        }
    }
}
