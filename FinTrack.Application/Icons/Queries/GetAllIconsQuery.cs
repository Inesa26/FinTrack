using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;

namespace FinTrack.Application.Icons.Queries
{
    public class GetAllIconsQuery: IRequest<PaginatedResult<IconDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public TransactionType TransactionType { get; set; }

        public GetAllIconsQuery(int pageIndex, int pageSize, TransactionType transactionType)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TransactionType = transactionType;
        }
    }
}
