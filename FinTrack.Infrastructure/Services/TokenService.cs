using FinTrack.Application.Abstractions;
using FinTrack.Domain.Model;
using System.Security.Claims;

namespace FinTrack.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IIdentityService _identityService;

        public TokenService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public string GenerateToken(ApplicationUser user)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            });

            var token = _identityService.CreateSecurityToken(identity);
            return _identityService.WriteToken(token);
        }
    }
}
