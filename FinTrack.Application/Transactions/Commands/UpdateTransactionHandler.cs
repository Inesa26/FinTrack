using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Commands;
public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTransactionHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTransactionHandler(IUnitOfWork unitOfWork, ILogger<UpdateTransactionHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
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

            _logger.LogInformation("Transaction with ID {TransactionId} updated successfully.", updatedTransaction.Id);
            return _mapper.Map<TransactionDto>(updatedTransaction);
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Failed to update transaction.");
            throw;
        }
    }
}
