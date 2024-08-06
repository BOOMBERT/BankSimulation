using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.User;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginUserDto userToAuth);
        Task<(AccessTokenDto, RefreshTokenDto)> RefreshTokensAsync(string accessToken, string? refreshToken);
    }
}
