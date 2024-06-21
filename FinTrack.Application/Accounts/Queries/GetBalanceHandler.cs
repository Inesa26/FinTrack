using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Accounts.Queries
{
    public class GetAccountBalanceQuery : IRequest<AccountDto>
    {
        public int AccountId { get; set; }
    }

    public class GetBalanceHandler : IRequestHandler<GetBalanceQuery, AccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBalanceHandler> _logger;

        public GetBalanceHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetBalanceHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AccountDto> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _unitOfWork.AccountRepository.Get(request.AccountId);
                if (account == null)
                {
                    _logger.LogWarning("Account with ID {AccountId} was not found", request.AccountId);
                    return null;
                }

                var accountDto = _mapper.Map<AccountDto>(account);
                return accountDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch account balance for AccountId: {AccountId}", request.AccountId);
                throw;
            }
        }
    }
}
