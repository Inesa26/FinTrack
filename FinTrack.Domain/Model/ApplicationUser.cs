using Microsoft.AspNetCore.Identity;

namespace FinTrack.Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public virtual Account? Account { get; set; }
    }
}
