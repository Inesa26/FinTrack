using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Categories.Commands;
public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCategoryHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateCategoryHandler(IUnitOfWork unitOfWork, ILogger<UpdateCategoryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingCategory = await _unitOfWork.CategoryRepository.Get(request.CategoryId) ??
                throw new InvalidOperationException($"Category with id'{request.CategoryId}' was not found.");

            var existingIcon = await _unitOfWork.IconRepository.Get(request.IconId) ??
                throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");

            existingCategory.IconId = request.IconId;
            existingCategory.Title = request.Title;
            existingCategory.Type = request.Type;

            var updatedCategory = await _unitOfWork.CategoryRepository.Update(request.CategoryId, existingCategory);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Category '{CategoryTitle}' updated successfully.", request.Title);
            return _mapper.Map<CategoryDto>(updatedCategory);
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to update category '{CategoryTitle}'.", request.Title);
            throw;
        }
    }
}
