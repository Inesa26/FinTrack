using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Commands;
public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTransactionHandler> _logger;
    private readonly IMapper _mapper;

    public DeleteTransactionHandler(IUnitOfWork unitOfWork, ILogger<DeleteTransactionHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
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

            _logger.LogInformation("Transaction with ID {TransactionId} removed successfully", request.TransactionId);
            return _mapper.Map<TransactionDto>(deletedTransaction);
        }

        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError("Failed to remove transaction with ID {TransactionId}", request.TransactionId);
            throw;
        }
    }
}
