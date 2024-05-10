using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Categories.Queries;
public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllCategoriesHandler> _logger;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(IUnitOfWork unitOfWork, ILogger<GetAllCategoriesHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _unitOfWork.CategoryRepository.GetAll();
            _logger.LogInformation("All categories listed successfully.");
            return categories.Select(each => _mapper.Map<CategoryDto>(each)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list all categories.");
            throw;
        }
    }
}