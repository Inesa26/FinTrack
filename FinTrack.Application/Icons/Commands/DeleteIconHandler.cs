using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Icons.Commands;

public class RemoveIconHandler : IRequestHandler<DeleteIconCommand, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveIconHandler> _logger;
    private readonly IMapper _mapper;

    public RemoveIconHandler(IUnitOfWork unitOfWork, ILogger<RemoveIconHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(DeleteIconCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingIcon = (await _unitOfWork.IconRepository.Get(request.IconId)) ??
                throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");

            var deletedIcon = await _unitOfWork.IconRepository.Delete(existingIcon);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Icon with ID {IconId} removed successfully.", request.IconId);

            return _mapper.Map<IconDto>(request);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to remove icon with ID: {IconId}", request.IconId);
            throw;
        }
    }
}


