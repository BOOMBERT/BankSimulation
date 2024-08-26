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
        Task<AuthUserDto?> GetUserAuthDataByEmailAsync(string email);
        Task<IList<AccessRole>?> GetUserAccessRolesAsync(Guid userId);
        Task<Guid?> GetUserIdByEmailAsync(string email);
        Task<bool> UserAlreadyDeletedByIdAsync(Guid userId);
        Task DeleteUserByIdAsync(Guid userId);
        Task UpdateUserPasswordAsync(Guid userId, string newPassword);
        Task<bool> SaveChangesAsync();
    }
}
