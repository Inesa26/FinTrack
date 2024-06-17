using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;

namespace FinTrack.Application.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<PaginatedResult<CategoryIconDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public TransactionType TransactionType { get; set; }

        public GetAllCategoriesQuery(int pageIndex, int pageSize, TransactionType transactionType)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TransactionType = transactionType;
        }
    }
}
