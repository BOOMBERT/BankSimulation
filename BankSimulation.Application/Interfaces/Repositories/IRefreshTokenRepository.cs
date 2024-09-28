using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshTokenDto?> GetAsync(Guid userId);
        Task DeleteAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(Guid userId);
    }
}
