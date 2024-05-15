using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FinTrack.Application.Abstractions
{
    public interface IIdentityService
    {
        SecurityToken CreateSecurityToken(ClaimsIdentity identity);
        string WriteToken(SecurityToken token);
    }
}
