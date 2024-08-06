using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> EmailAlreadyExistsAsync(string email);
        Task AddUserRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetUserRefreshTokenAsync(Guid userId);
        void DeleteUserRefreshToken(RefreshToken refreshToken);
        Task<bool> SaveChangesAsync();
    }
}
