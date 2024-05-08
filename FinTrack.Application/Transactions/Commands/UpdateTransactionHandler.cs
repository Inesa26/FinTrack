using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Commands;
public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTransactionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var existingTransaction = (await _unitOfWork.TransactionRepository.Get(request.TransactionId)) ??
                throw new InvalidOperationException($"Transaction with ID '{request.TransactionId}' was not found.");

            var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

            existingTransaction.Date = request.Date;
            existingTransaction.Description = request.Description;
            existingTransaction.Amount = request.Amount;
            existingTransaction.CategoryId = request.CategoryId;

            var updatedTransaction = await _unitOfWork.TransactionRepository.Update(request.TransactionId, existingTransaction);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return TransactionDto.FromTransaction(updatedTransaction);
        }

        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
