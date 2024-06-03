using FinTrack.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinTrack.Application.Auth.Commands
{
    public class SignInHandler : IRequestHandler<SignInCommand, string>
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<SignInHandler> _logger;


        public SignInHandler(ILogger<SignInHandler> logger, IUserAuthenticationService userAuthenticationService, ITokenService tokenService)
        {
            _userAuthenticationService = userAuthenticationService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userAuthenticationService.AuthenticateAsync(request.Email, request.Password);
            if (user == null) { return null; }

            string? accessToken = null;
            accessToken = _tokenService.GenerateToken(user);
            return accessToken;
        }
    }
}