using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Commands;
public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingTransaction = (await _unitOfWork.TransactionRepository.Get(request.TransactionId)) ??
                throw new InvalidOperationException($"Transaction with ID '{request.TransactionId}' was not found.");

            var deletedTransaction = await _unitOfWork.TransactionRepository.Delete(existingTransaction);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return TransactionDto.FromTransaction(deletedTransaction);
        }

        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
