using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Commands;
public class UpdateIconHandler : IRequestHandler<UpdateIconCommand, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateIconHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

            return IconDto.FromIcon(updatedIcon);
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


