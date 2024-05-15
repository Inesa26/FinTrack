using FinTrack.Domain.Model;

namespace FinTrack.Application.Abstractions
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
