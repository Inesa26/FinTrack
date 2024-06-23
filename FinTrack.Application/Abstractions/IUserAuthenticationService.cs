using FinTrack.Domain.Model;

namespace FinTrack.Application.Abstractions
{
    public interface IUserAuthenticationService
    {
        Task<ApplicationUser?> AuthenticateAsync(string email, string password);
    }
}
