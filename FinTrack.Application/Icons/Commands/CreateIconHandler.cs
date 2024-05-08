using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;
using MediatR;

namespace FinTrack.Application.Icons.Commands;
public class CreateIconHandler : IRequestHandler<CreateIconCommand, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateIconHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IconDto> Handle(CreateIconCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            byte[] image = ReadImageFile(request.FilePath);
            Icon icon = new(image);
            var createdIcon = await _unitOfWork.IconRepository.Add(icon);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return IconDto.FromIcon(createdIcon);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public byte[] ReadImageFile(string filePath)
    {

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File '{filePath}' not found.");
        }

        byte[] imageData = File.ReadAllBytes(filePath);

        return imageData;
    }
}


