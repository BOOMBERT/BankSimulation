using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using System.Security.Claims;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAuthService
    {
        IEnumerable<Claim> GetAllClaimsFromJwt(string token);
        Claim GetSpecificClaimFromJwt(string token, string claimName);
        string GenerateAccessToken(Guid userId, IEnumerable<AccessRole> userAccessRoles);
        RefreshToken GenerateRefreshToken();
        bool VerifyUserPassword(string plainPassword, string passwordHash);
    }
}
