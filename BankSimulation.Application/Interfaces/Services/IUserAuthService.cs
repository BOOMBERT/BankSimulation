using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.User;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IUserAuthService
    {
        Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginUserDto userToAuth);
        Task<(AccessTokenDto, RefreshTokenDto)> RefreshUserTokensAsync(string accessToken, string? refreshToken);
        Guid GetUserIdFromJwt(string token);
    }
}
