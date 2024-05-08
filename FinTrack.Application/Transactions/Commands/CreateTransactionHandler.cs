using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;
using MediatR;

namespace FinTrack.Application.Transactions.Commands;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");
            
            Transaction transaction = new(request.Amount, request.Date, request.Description, request.CategoryId);

            var createdTransaction = await _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitTransactionAsync();

            return TransactionDto.FromTransaction(createdTransaction);
        }

        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
