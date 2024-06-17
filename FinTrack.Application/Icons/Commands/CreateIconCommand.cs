using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;

namespace FinTrack.Application.Icons.Commands;

public class CreateIconCommand : IRequest<IconDto>
{
    public CreateIconCommand(string filePath, TransactionType type, string title)
    {
        FilePath = filePath;
        TransactionType = type;
        Title = title;
    }
    public string FilePath { get; set; }
    public TransactionType TransactionType { get; set; }
    public string Title { get; set; }
}


