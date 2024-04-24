using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Icons.Commands;
public record DeleteIcon(int IconId) : IRequest<IconDto>;
public class RemoveIconHandler : IRequestHandler<DeleteIcon, IconDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveIconHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IconDto> Handle(DeleteIcon request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingIcon = (await _unitOfWork.IconRepository.Get(request.IconId)) ??
                throw new InvalidOperationException($"Icon with ID '{request.IconId}' was not found.");

            var deletedIcon = await _unitOfWork.IconRepository.Delete(existingIcon);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return IconDto.FromCategory(deletedIcon);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}


