using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetAllTransactionsQuery : IRequest<PaginatedResult<TransactionDto>>
    {
        public int AccountId { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public string SortBy { get; }
        public string SortOrder { get; }

        public GetAllTransactionsQuery(int accountId, int pageIndex, int pageSize, string sortBy, string sortOrder)
        {
            AccountId = accountId;
            PageIndex = pageIndex;
            PageSize = pageSize;
            SortBy = sortBy;
            SortOrder = sortOrder;
        }
    }
}
