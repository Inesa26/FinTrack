using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Categories.Queries;
public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCategoryByIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(IUnitOfWork unitOfWork, ILogger<GetCategoryByIdHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");
            _logger.LogInformation("Category with ID {CategoryId} found successfully.", request.CategoryId);
            return _mapper.Map<CategoryDto>(existingCategory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to find category by ID: {CategoryId}", request.CategoryId);
            throw;
        }
    }
}