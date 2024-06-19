using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Commands
{
    public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionCommand, TransactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteTransactionHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMonthlySummaryService _monthlySummaryService;

        public DeleteTransactionHandler(IUnitOfWork unitOfWork, ILogger<DeleteTransactionHandler> logger, IMapper mapper, IMonthlySummaryService monthlySummaryService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _monthlySummaryService = monthlySummaryService;
        }

        public async Task<TransactionDto> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingTransaction = await _unitOfWork.TransactionRepository.Get(request.TransactionId)
                    ?? throw new InvalidOperationException($"Transaction with ID '{request.TransactionId}' was not found.");

                var account = await _unitOfWork.AccountRepository.Get(existingTransaction.AccountId)
                    ?? throw new InvalidOperationException($"Account with ID '{existingTransaction.AccountId}' was not found.");

                // Calculate the transaction amount for updating the account balance and monthly summary
                decimal transactionAmount = existingTransaction.Type == TransactionType.Expense ? -existingTransaction.Amount : existingTransaction.Amount;

                // Update account balance
                account.Balance -= transactionAmount;

                // Update monthly summary
                await _monthlySummaryService.UpdateMonthlySummary(existingTransaction.AccountId, existingTransaction.Date.Year, (Month)existingTransaction.Date.Month, -transactionAmount);

                var deletedTransaction = await _unitOfWork.TransactionRepository.Delete(existingTransaction);

                // Save changes
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Transaction with ID {TransactionId} removed successfully", request.TransactionId);
                return _mapper.Map<TransactionDto>(deletedTransaction);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to remove transaction with ID {TransactionId}", request.TransactionId);
                throw;
            }
        }
    }
}
