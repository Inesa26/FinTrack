using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Categories.Commands;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCategoryHandler> _logger;
    private readonly IMapper _mapper;
    public DeleteCategoryHandler(IUnitOfWork unitOfWork, ILogger<DeleteCategoryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingCategory = await _unitOfWork.CategoryRepository.Get(request.CategoryId) ??
                throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

            var deletedCategory = await _unitOfWork.CategoryRepository.Delete(existingCategory);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Category with ID {CategoryId} deleted successfully.", request.CategoryId);
            return _mapper.Map<CategoryDto>(deletedCategory);
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to delete category with ID {CategoryId}.", request.CategoryId);
            throw;
        }
    }
}