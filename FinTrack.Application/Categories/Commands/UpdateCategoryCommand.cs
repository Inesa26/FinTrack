using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;

namespace FinTrack.Application.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<CategoryDto>
    {
        public UpdateCategoryCommand(int categoryId, string title, int iconId, TransactionType type)
        {
            CategoryId = categoryId;
            Title = title;
            IconId = iconId;
            Type = type;
        }

        public int CategoryId { get; set; }
        public string Title { get; set; }
        public int IconId { get; set; }
        public TransactionType Type { get; set; }
    }
}
