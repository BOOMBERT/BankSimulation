using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IUserAuthService
    {
        Task<(AccessTokenDto, RefreshTokenDto)> AuthenticateUserAsync(LoginUserDto userToAuth);
        Task<(AccessTokenDto, RefreshTokenDto)> RefreshUserTokensAsync(string accessToken, string? refreshToken);
        Task<User> GetUserEntityFromJwtAsync(string token);
        Guid GetUserIdFromJwt(string token);
    }
}
