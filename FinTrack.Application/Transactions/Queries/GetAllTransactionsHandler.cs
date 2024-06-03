using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, PaginatedResult<TransactionDto>>
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

        public async Task<PaginatedResult<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
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

                var transactionDtos = pagedTransactions.Select(each => _mapper.Map<TransactionDto>(each)).ToList();
                var paginatedResult = new PaginatedResult<TransactionDto>(transactionDtos, totalCount, request.PageIndex, request.PageSize);

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
