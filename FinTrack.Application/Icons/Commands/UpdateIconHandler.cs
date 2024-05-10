using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Icons.Commands;
public class UpdateIconHandler : IRequestHandler<UpdateIconCommand, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveIconHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateIconHandler(IUnitOfWork unitOfWork, ILogger<RemoveIconHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(UpdateIconCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingIcon = (await _unitOfWork.IconRepository.Get(request.IconId)) ??
                throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");
            byte[] image = ReadImageFile(request.FilePath);

            existingIcon.Data = image;

            var updatedIcon = await _unitOfWork.IconRepository.Update(request.IconId, existingIcon);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Icon with ID {IconId} created successfully.", updatedIcon.Id);
           return _mapper.Map<IconDto>(updatedIcon);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to update icon with id {IconId}.", request.IconId);
            throw;
        }
    }

    public byte[] ReadImageFile(string filePath)
    {

        if (!File.Exists(filePath))
        {
            _logger.LogError("File '{FilePath}' not found.", filePath);
            throw new FileNotFoundException($"File '{filePath}' not found.");
        }

        byte[] imageData = File.ReadAllBytes(filePath);

        return imageData;
    }
}


