using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Auth.Commands
{
    public class LogInHandler : IRequestHandler<LogInCommand, LoginDto>
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LogInHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public LogInHandler(ILogger<LogInHandler> logger, IUserAuthenticationService userAuthenticationService,
            ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _userAuthenticationService = userAuthenticationService;
            _tokenService = tokenService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginDto> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userAuthenticationService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
            { return null; }
            Account account = await _unitOfWork.AccountRepository.GetSingle(q => q.Where(a => a.UserId == user.Id))
                ?? throw new InvalidOperationException($"Account for user with ID '{user.Id}' was not found.");

            var accessToken = _tokenService.GenerateToken(user);
            if (accessToken != null && account != null)
            {
                LoginDto loginDto = new LoginDto { AccountId = account.Id, Token = accessToken };
                return loginDto;
            }
            else
            {
                return null;
            }
        }
    }
}