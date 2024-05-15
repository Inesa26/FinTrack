using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "CategoryId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(20, ErrorMessage = "Title cannot exceed 20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "IconId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "IconId must be greater than 0")]
        public int IconId { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public TransactionType Type { get; set; }
    }
}
