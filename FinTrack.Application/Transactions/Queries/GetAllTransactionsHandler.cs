using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Queries;
public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTransactionsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.TransactionRepository.GetAll();
        return transactions.Select(TransactionDto.FromTransaction).ToList();
    }
}
