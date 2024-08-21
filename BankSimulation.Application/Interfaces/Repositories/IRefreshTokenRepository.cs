using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshTokenDto?> GetRefreshTokenByUserIdAsync(Guid userId);
        Task DeleteRefreshTokenByUserIdAsync(Guid userId);
    }
}
