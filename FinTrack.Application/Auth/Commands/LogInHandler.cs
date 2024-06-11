using FinTrack.Application.Abstractions;
using FinTrack.Application.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Auth.Commands
{
    public class LogInHandler : IRequestHandler<LogInCommand, LoginDto>
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LogInHandler> _logger;


        public LogInHandler(ILogger<LogInHandler> logger, IUserAuthenticationService userAuthenticationService, ITokenService tokenService)
        {
            _userAuthenticationService = userAuthenticationService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginDto> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userAuthenticationService.AuthenticateAsync(request.Email, request.Password);
            if (user == null) { return null; }

            var accessToken = _tokenService.GenerateToken(user);
            if (accessToken != null)
            {
                LoginDto loginDto = new LoginDto { Token = accessToken };
                return loginDto;
            }
            else
            {
                return null;
            }
        }
    }
}