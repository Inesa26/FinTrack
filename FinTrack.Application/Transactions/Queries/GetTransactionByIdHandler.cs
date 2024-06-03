using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Queries;
public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTransactionByIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetTransactionByIdHandler(IUnitOfWork unitOfWork, ILogger<GetTransactionByIdHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var existingTransaction = (await _unitOfWork.TransactionRepository.Get(request.TransactionId)) ??
                throw new InvalidOperationException($"Transaction with ID '{request.TransactionId}' was not found.");
            _logger.LogInformation("Transaction with id {TransactionId} found successfully", request.TransactionId);
            return _mapper.Map<TransactionDto>(existingTransaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to find transaction by ID {TransactionId}", request.TransactionId);
            throw;
        }
    }
}

