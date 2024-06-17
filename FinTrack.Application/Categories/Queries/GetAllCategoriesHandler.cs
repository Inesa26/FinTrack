using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Common.Models;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Categories.Queries;
public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, PaginatedResult<CategoryIconDto>>
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

    public async Task<PaginatedResult<CategoryIconDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve paginated categories from the repository
            var paginatedResult = await _unitOfWork.CategoryRepository.GetPaginated(
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                query => query.Where(category => category.Type == request.TransactionType));

            _logger.LogInformation("All categories listed successfully.");

            // Fetch icons for each category
            var categoryDtos = new List<CategoryIconDto>();
            foreach (var category in paginatedResult.Items)
            {
                var categoryDto = _mapper.Map<CategoryIconDto>(category);
                var icon = await _unitOfWork.IconRepository.Get(category.IconId); 

                categoryDto.Icon = _mapper.Map<IconDto>(icon);
                categoryDtos.Add(categoryDto);
            }

            return new PaginatedResult<CategoryIconDto>(categoryDtos, paginatedResult.TotalCount, request.PageIndex, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list all categories.");
            throw;
        }
    }
}
 