using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Commands;

public class CreateIconCommand : IRequest<IconDto>
{
    public CreateIconCommand(string filePath)
    {
        FilePath = filePath;
    }
    public string FilePath { get; set; }
}


