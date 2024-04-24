using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;

namespace FinTrack.Application.Categories.Commands;
public record UpdateCategory(int CategoryId, string Title, int IconId, TransactionType Type) : IRequest<CategoryDto>;
public class UpdateCategoryHandler : IRequestHandler<UpdateCategory, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto> Handle(UpdateCategory request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                throw new InvalidOperationException($"Category with id'{request.CategoryId}' was not found.");

            var existingIcon = (await _unitOfWork.IconRepository.Get(request.IconId)) ??
                throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");

            existingCategory.Icon = existingIcon;
            existingCategory.Title = request.Title;
            existingCategory.Type = request.Type;

            var updatedCategory = await _unitOfWork.CategoryRepository.Update(request.CategoryId, existingCategory);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return CategoryDto.FromCategory(updatedCategory);
        }

        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
