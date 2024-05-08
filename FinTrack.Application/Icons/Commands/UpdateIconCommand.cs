using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Commands
{
    public class UpdateIconCommand : IRequest<IconDto>
    {
        public UpdateIconCommand(int iconId, string filePath)
        {
            IconId = iconId;
            FilePath = filePath;
        }

        public int IconId { get; set; }
        public string FilePath { get; set; }
    }

}
