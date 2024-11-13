using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetAsync(Guid id, bool trackChanges);
        Task<User?> GetAsync(string email, bool trackChanges);
        Task<string?> GetPasswordAsync(Guid userId);
        Task<string?> GetEmailAsync(Guid userId);
        Task<IEnumerable<AccessRole>> GetAccessRolesAsync(Guid userId);
        Task UpdateAsync(Guid userId, string newFirstName, string newLastName, string newEmail, string newPassword);
        Task UpdatePasswordAsync(Guid userId, string newPassword);
        Task UpdateEmailAsync(Guid userId, string newEmail);
        Task SoftDeleteAsync(Guid userId);
        Task<bool> AlreadyDeletedAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(string email);
        Task<bool> AlreadyExistsExceptSpecificUserAsync(string email, Guid userIdToExcept);
        Task<bool> SaveChangesAsync();
    }
}
