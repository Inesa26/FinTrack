using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using MediatR;

namespace FinTrack.Application.Categories.Queries;
public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
       
            var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

            return CategoryDto.FromCategory(existingCategory);
    }
}