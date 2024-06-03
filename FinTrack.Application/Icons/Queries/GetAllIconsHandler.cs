using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Common.Models;
using FinTrack.Application.Icons.Commands;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Icons.Queries;
public class GetAllIconsHandler : IRequestHandler<GetAllIconsQuery, PaginatedResult<IconDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllIconsHandler> _logger;
    private readonly IMapper _mapper;

    public GetAllIconsHandler(IUnitOfWork unitOfWork, ILogger<GetAllIconsHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<IconDto>> Handle(GetAllIconsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var paginatedResult = await _unitOfWork.IconRepository.GetPaginated(pageIndex: request.PageIndex, pageSize: request.PageSize);

            _logger.LogInformation("All icons listed successfully.");

            var mappedIcons = paginatedResult.Items.Select(each => _mapper.Map<IconDto>(each)).ToList();

            return new PaginatedResult<IconDto>(mappedIcons, paginatedResult.TotalCount, request.PageIndex, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list all icons.");
            throw;
        }
    }

}

