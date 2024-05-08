using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Categories.Queries;
public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCategoriesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CategoryRepository.GetAll();
        return categories.Select(CategoryDto.FromCategory).ToList();
    }
}