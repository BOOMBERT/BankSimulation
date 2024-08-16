using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> EmailAlreadyExistsAsync(string email);
        Task<AuthUserDto?> GetUserAuthDataAsync(string email);
        Task AddUserRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshTokenDto?> GetRefreshTokenByUserIdAsync(Guid userId);
        Task DeleteRefreshTokenByUserIdAsync(Guid userId);
        Task<IList<AccessRole>?> GetUserAccessRolesAsync(Guid userId);
        Task<bool> SaveChangesAsync();
    }
}
