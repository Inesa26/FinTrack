using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Commands
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTransactionHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMonthlySummaryService _monthlySummaryService;

        public CreateTransactionHandler(IUnitOfWork unitOfWork, ILogger<CreateTransactionHandler> logger, IMapper mapper, IMonthlySummaryService monthlySummaryService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _monthlySummaryService = monthlySummaryService;
        }

        public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingCategory = await _unitOfWork.CategoryRepository.Get(request.CategoryId)
                    ?? throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

                var account = await _unitOfWork.AccountRepository.Get(request.AccountId)
                    ?? throw new InvalidOperationException($"Account with ID '{request.AccountId}' was not found.");

                // Calculate transaction amount based on transaction type
                decimal transactionAmount = request.TransactionType == TransactionType.Expense ? -request.Amount : request.Amount;

                // Create transaction entity
                Transaction transaction = new Transaction(request.AccountId, request.Amount, request.Date,
                    request.Description, request.CategoryId, request.TransactionType);

                var createdTransaction = await _unitOfWork.TransactionRepository.Add(transaction);

                // Update account balance
                account.Balance += transactionAmount;

                // Update monthly summary
                await _monthlySummaryService.UpdateMonthlySummary(request.AccountId, request.Date.Year, (Month)request.Date.Month, transactionAmount);

                // Save changes
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Transaction with ID {TransactionId} created successfully.", createdTransaction.Id);

                return _mapper.Map<TransactionDto>(createdTransaction);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create transaction.");
                throw;
            }
        }
    }
}
