using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;

namespace FinTrack.Application.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public CreateCategoryCommand(string title, int iconId, TransactionType type)
        {
            Title = title;
            IconId = iconId;
            Type = type;
        }

        public string Title { get; set; }
        public int IconId { get; set; }
        public TransactionType Type { get; set; }
    }
}
