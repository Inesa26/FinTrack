using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetAllTransactionsQuery : IRequest<PaginatedResult<TransactionCategoryDto>>
    {
        [Required(ErrorMessage = "AccountId is required")]
        public int AccountId { get; }

        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be at least 1")]
        public int PageIndex { get; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be at least 1")]
        public int PageSize { get; }

        [Required(ErrorMessage = "SortBy is required")]
        [MaxLength(50, ErrorMessage = "SortBy cannot exceed 50 characters")]
        public string SortBy { get; }

        [Required(ErrorMessage = "SortOrder is required")]
        [RegularExpression("^(asc|desc)$", ErrorMessage = "SortOrder must be 'asc' or 'desc'")]
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
