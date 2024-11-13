using BankSimulation.Domain.Entities;

namespace BankSimulation.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetAsync(Guid userId, bool trackChanges);
        Task UpdateAsync(Guid userId, string newRefreshToken, DateTime newExpirationDate);
        Task<bool> AlreadyExistsAsync(Guid userId);
    }
}
