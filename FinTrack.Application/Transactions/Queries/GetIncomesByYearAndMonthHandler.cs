using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetIncomesByTransactionTypeHandler : IRequestHandler<GetIncomesByYearAndMonthQuery, List<IncomeExpensesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetIncomesByTransactionTypeHandler> _logger;

        public GetIncomesByTransactionTypeHandler(IUnitOfWork unitOfWork, ILogger<GetIncomesByTransactionTypeHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<IncomeExpensesDto>> Handle(GetIncomesByYearAndMonthQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var startDate = new DateTime(request.Year, request.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var transactions = await _unitOfWork.TransactionRepository
                    .Filter(q => q.Where(t =>
                        t.Date >= startDate &&
                        t.Date <= endDate &&
                        t.Type == TransactionType.Income &&
                        t.AccountId == request.AccountId));

                var incomeGroupedByCategory = transactions
                    .GroupBy(t => t.CategoryId)
                    .Select(g => new
                    {
                        CategoryId = g.Key,
                        TotalAmount = g.Sum(t => t.Amount)
                    });

                var incomeDtos = new List<IncomeExpensesDto>();
                foreach (var group in incomeGroupedByCategory)
                {
                    var category = await _unitOfWork.CategoryRepository.Get(group.CategoryId);
                    if (category != null)
                    {
                        var incomeDto = new IncomeExpensesDto
                        {
                            Amount = group.TotalAmount,
                            CategoryName = category.Title
                        };
                        incomeDtos.Add(incomeDto);
                    }
                }

                _logger.LogInformation("Incomes for year {Year} and month {Month} retrieved successfully.", request.Year, request.Month);
                return incomeDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve incomes for year {Year} and month {Month}.", request.Year, request.Month);
                throw;
            }
        }
    }
}
