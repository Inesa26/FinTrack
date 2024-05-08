using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<CategoryDto>
    {
        public DeleteCategoryCommand(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; set; }
    }
}
