using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Icons.Commands;

public class CreateIconCommand : IRequest<IconDto>
{
    public CreateIconCommand(string filePath, TransactionType type, string title)
    {
        FilePath = filePath;
        TransactionType = type;
        Title = title;
    }
    [Required(ErrorMessage = "FilePath is required")]
    public string FilePath { get; set; }
    [Required(ErrorMessage = "Transaction type is required")]
    public TransactionType TransactionType { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
}


