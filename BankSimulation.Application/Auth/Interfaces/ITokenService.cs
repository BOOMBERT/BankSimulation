using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BankSimulation.Application.Auth.Interfaces
{
    public interface ITokenService
    {
        IEnumerable<Claim> GetAllClaimsFromJwt(string token);
        Claim GetSpecificClaimFromJwt(string token, string claimName);
        Guid GetUserIdFromJwt(string token);
        string GenerateAccessToken(Guid userId, IEnumerable<AccessRole> userAccessRoles);
        RefreshToken GenerateRefreshToken();
    }
}
