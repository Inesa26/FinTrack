using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Queries
{
    public class GetIconByIdQuery : IRequest<IconDto>
    {
        public GetIconByIdQuery(int iconId)
        {
            IconId = iconId;
        }

        public int IconId { get; set; }
    }
}
