using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Commands
{
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateTransactionHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMonthlySummaryService _monthlySummaryService;

        public UpdateTransactionHandler(IUnitOfWork unitOfWork, ILogger<UpdateTransactionHandler> logger, IMapper mapper, IMonthlySummaryService monthlySummaryService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _monthlySummaryService = monthlySummaryService;
        }

        public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingTransaction = await _unitOfWork.TransactionRepository.Get(request.TransactionId)
                    ?? throw new InvalidOperationException($"Transaction with ID '{request.TransactionId}' was not found.");

                var existingCategory = await _unitOfWork.CategoryRepository.Get(request.CategoryId)
                    ?? throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

                var account = await _unitOfWork.AccountRepository.Get(existingTransaction.AccountId)
                    ?? throw new InvalidOperationException($"Account with ID '{existingTransaction.AccountId}' was not found.");

                // Calculate the difference in amount for updating the account balance and monthly summary
                decimal oldTransactionAmount = existingTransaction.Type == TransactionType.Expense ? -existingTransaction.Amount : existingTransaction.Amount;
                decimal newTransactionAmount = request.TransactionType == TransactionType.Expense ? -request.Amount : request.Amount;
                decimal amountDifference = newTransactionAmount - oldTransactionAmount;

                // Update account balance
                account.Balance += amountDifference;

                // Update transaction details
                existingTransaction.Date = request.Date;
                existingTransaction.Description = request.Description;
                existingTransaction.Amount = request.Amount;
                existingTransaction.CategoryId = request.CategoryId;
                existingTransaction.Type = request.TransactionType;

                var updatedTransaction = await _unitOfWork.TransactionRepository.Update(request.TransactionId, existingTransaction);

                // Update monthly summary
                await _monthlySummaryService.UpdateMonthlySummary(existingTransaction.AccountId, request.Date.Year, (Month)request.Date.Month, amountDifference);

                // Save changes
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
}
