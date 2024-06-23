using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Icons.Queries
{
    public class GetAllIconsQuery : IRequest<PaginatedResult<IconDto>>
    {
        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be at least 1")]
        [Required(ErrorMessage = "PageIndex is required")]
        public int PageIndex { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be at least 1")]
        [Required(ErrorMessage = "PageSize is required")]
        public int PageSize { get; set; }
        [Required(ErrorMessage = "TransactionType is required")]
        public TransactionType TransactionType { get; set; }

        public GetAllIconsQuery(int pageIndex, int pageSize, TransactionType transactionType)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TransactionType = transactionType;
        }
    }
}
