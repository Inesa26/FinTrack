using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Queries
{
    public class GetAllIconsQuery: IRequest<PaginatedResult<IconDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public GetAllIconsQuery(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
