using FinTrack.Application.Abstractions;
using FinTrack.Domain.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FinTrack.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> CreateUserAsync(string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName, LastName = lastName };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors)}");
            }

            var userRole = "User";
            if (!await _roleManager.RoleExistsAsync(userRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(userRole));
            }
            await _userManager.AddToRoleAsync(user, userRole);

            return user;
        }

        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims)
        {
            var result = await _userManager.AddClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to add claims to user: {string.Join(", ", result.Errors)}");
            }
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var result = await _userManager.GetClaimsAsync(user);
            return result.ToList();
        }
    }
}
