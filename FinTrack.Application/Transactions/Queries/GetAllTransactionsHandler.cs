using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, PaginatedResult<TransactionCategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllTransactionsHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllTransactionsHandler(IUnitOfWork unitOfWork, ILogger<GetAllTransactionsHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<TransactionCategoryDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var transactions = await _unitOfWork.TransactionRepository
                    .Filter(q => q.Where(t => t.AccountId == request.AccountId));

                // Sorting
                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    transactions = request.SortOrder?.ToLower() == "desc" ?
                    transactions.OrderByDescending(t => t.GetType().GetProperty(request.SortBy).GetValue(t, null)).ToList() :
                    transactions.OrderBy(t => t.GetType().GetProperty(request.SortBy).GetValue(t, null)).ToList();
                }

                // Pagination
                var totalCount = transactions.Count;
                var pagedTransactions = transactions
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var transactionDtos = new List<TransactionCategoryDto>();
                foreach (var transaction in pagedTransactions)
                {
                    var transactionDto = _mapper.Map<TransactionCategoryDto>(transaction);

                    // Fetch the category and its icon
                    var category = await _unitOfWork.CategoryRepository.Get(transaction.CategoryId);
                    if (category != null)
                    {
                        var icon = await _unitOfWork.IconRepository.Get(category.IconId);
                        var categoryDto = _mapper.Map<CategoryIconDto>(category);
                        categoryDto.Icon = _mapper.Map<IconDto>(icon);
                        transactionDto.Category = categoryDto;
                    }

                    transactionDtos.Add(transactionDto);
                }

                var paginatedResult = new PaginatedResult<TransactionCategoryDto>(transactionDtos, totalCount, request.PageIndex, request.PageSize);

                _logger.LogInformation("Transactions listed successfully.");
                return paginatedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to list all transactions.");
                throw;
            }
        }
    }
}
