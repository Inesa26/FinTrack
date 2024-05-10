using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Icons.Commands;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Icons.Queries;
public class GetAllIconsHandler : IRequestHandler<GetAllIconsQuery, List<IconDto>>
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

    public async Task<List<IconDto>> Handle(GetAllIconsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var icons = await _unitOfWork.IconRepository.GetAll();
            _logger.LogInformation("All icons listed successfully.");
            return icons.Select(each => _mapper.Map<IconDto>(each)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list all icons.");
            throw;
        }
    }
}

