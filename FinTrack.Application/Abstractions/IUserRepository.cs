using FinTrack.Domain.Model;
using System.Security.Claims;

namespace FinTrack.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task<ApplicationUser> CreateUserAsync(string email, string password, string firstName, string lastName);
        Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims);
        Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user);
        Task UpdateUserAsync(ApplicationUser user);
        Task ReplaceUserClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task SetPasswordAsync(ApplicationUser user, string newPassword);
    }
}