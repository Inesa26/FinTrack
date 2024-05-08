using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {
        public GetCategoryByIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; set; }
    }
}
