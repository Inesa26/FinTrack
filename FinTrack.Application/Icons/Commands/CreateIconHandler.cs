using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Icons.Commands;
public class CreateIconHandler : IRequestHandler<CreateIconCommand, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateIconHandler> _logger;
    private readonly IMapper _mapper;

    public CreateIconHandler(IUnitOfWork unitOfWork, ILogger<CreateIconHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IconDto> Handle(CreateIconCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            byte[] image = ReadImageFile(request.FilePath);
            Icon icon = new(image, request.TransactionType, request.Title);
            var createdIcon = await _unitOfWork.IconRepository.Add(icon);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Icon with ID {IconId} created successfully.", createdIcon.Id);
            return _mapper.Map<IconDto>(icon);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to create icon.");
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


