using BankSimulation.Application.Auth.Dtos;

namespace BankSimulation.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginDto userToAuth);
        Task<(AccessTokenDto, RefreshTokenDto)> RefreshUserTokensAsync(string accessToken, string? refreshToken);
    }
}
