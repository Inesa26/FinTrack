using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Icons.Queries;
public class GetIconByIdHandler : IRequestHandler<GetIconByIdQuery, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetIconByIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetIconByIdHandler(IUnitOfWork unitOfWork, ILogger<GetIconByIdHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(GetIconByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var existingIcon = (await _unitOfWork.IconRepository.Get(request.IconId)) ??
                throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");
            _logger.LogInformation("Icon with id {IconId} found successfully", request.IconId);
            return _mapper.Map<IconDto>(existingIcon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to find icon by ID {IconId}", request.IconId);
            throw;
        }
    }
}

