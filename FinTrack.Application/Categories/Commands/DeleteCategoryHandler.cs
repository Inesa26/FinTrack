using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Categories.Commands;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

            var deletedCategory = await _unitOfWork.CategoryRepository.Delete(existingCategory);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return CategoryDto.FromCategory(deletedCategory);
        }

        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}