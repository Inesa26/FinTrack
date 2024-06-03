using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Accounts.Commands
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAccountHandler> _logger;
        private readonly IMapper _mapper;

        public CreateAccountHandler(IUnitOfWork unitOfWork, ILogger<CreateAccountHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id) ??
                    throw new InvalidOperationException($"User with ID '{request.Id}' was not found.");

                Account account = new(request.Id);

                var createdAccount = await _unitOfWork.AccountRepository.Add(account);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Account for UserId: '{UserId}' created successfully.", request.Id);
                return _mapper.Map<AccountDto>(account);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create account for UserId: '{UserId}'.", request.Id);
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
