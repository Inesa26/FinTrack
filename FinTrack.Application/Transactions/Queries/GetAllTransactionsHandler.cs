using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Transactions.Queries;
public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllTransactionsHandler> _logger;
    private readonly IMapper _mapper;


    public GetAllTransactionsHandler(IUnitOfWork unitOfWork, ILogger<GetAllTransactionsHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAll();
            _logger.LogInformation("All transactions listed successfully.");
            return transactions.Select(each => _mapper.Map<TransactionDto>(each)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list all transactions.");
            throw;
        }
    }
}
