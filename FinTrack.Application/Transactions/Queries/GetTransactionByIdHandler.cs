using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Queries;
public class GetTransacionByIdHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTransacionByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {

        var existingTransaction = (await _unitOfWork.TransactionRepository.Get(request.TransactionId)) ??
            throw new InvalidOperationException($"Transaction with ID '{request.TransactionId}' was not found.");

        return TransactionDto.FromTransaction(existingTransaction);
    }
}

