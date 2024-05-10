using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Categories.Commands;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCategoryHandler> _logger;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(IUnitOfWork unitOfWork, ILogger<CreateCategoryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingCategory = (await _unitOfWork.CategoryRepository.GetAll())
                .FirstOrDefault(c => c.Title.ToLowerInvariant() == request.Title.ToLowerInvariant());

            if (existingCategory is not null)
            {
                _logger.LogWarning("Category '{CategoryTitle}' already exists.", request.Title);
                throw new InvalidOperationException($"Category '{request.Title}' already exists.");
            }
            var existingIcon = await _unitOfWork.IconRepository.Get(request.IconId) ??
                throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");

            Category category = new(request.Title, request.Type, request.IconId);

            var createdCategory = await _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Category '{CategoryTitle}' created successfully.", request.Title);
            return _mapper.Map<CategoryDto>(category);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create category '{CategoryTitle}'.", request.Title);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}