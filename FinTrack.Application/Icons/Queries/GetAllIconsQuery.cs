using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Queries
{
    public class GetAllIconsQuery: IRequest<List<IconDto>>
    {
    }
}
