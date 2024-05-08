using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
    }
}
