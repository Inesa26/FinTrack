using FinTrack.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Application.Abstractions
{
    public interface IUserAuthenticationService
    {
        Task<ApplicationUser?> AuthenticateAsync(string email, string password);
    }
}
