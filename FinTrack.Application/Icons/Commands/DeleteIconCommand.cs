using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Commands
{
    public class DeleteIconCommand : IRequest<IconDto>
    {
        public DeleteIconCommand(int iconId)
        {
            IconId = iconId;
        }

        public int IconId { get; set; }
    }

}
