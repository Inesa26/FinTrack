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

        public CreateTransactionHandler(IUnitOfWork unitOfWork, ILogger<CreateTransactionHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingCategory = (await _unitOfWork.CategoryRepository.Get(request.CategoryId)) ??
                    throw new InvalidOperationException($"Category with ID '{request.CategoryId}' was not found.");

                var account = await _unitOfWork.AccountRepository.Get(request.AccountId) ??
                    throw new InvalidOperationException($"Account with ID '{request.AccountId}' was not found.");

                if (request.TransactionType == TransactionType.Expense)
                {
                    account.Balance -= request.Amount;
                }
                else if (request.TransactionType == TransactionType.Income)
                {
                    account.Balance += request.Amount;
                }

                Transaction transaction = new(request.AccountId, request.Amount, request.Date,
                    request.Description, request.CategoryId, request.TransactionType);

                var createdTransaction = await _unitOfWork.TransactionRepository.Add(transaction);

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
