using BankSimulation.Application.Dtos.User;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetAsync(Guid id);
        Task<UserDto?> GetDtoAsync(Guid id);
        Task<UserDto?> GetDtoAsync(string email);
        Task<AuthUserDto?> GetAuthDataAsync(string email);
        Task<string?> GetPasswordAsync(Guid userId);
        Task<string?> GetEmailAsync(Guid userId);
        Task<IEnumerable<AccessRole>?> GetAccessRolesAsync(Guid userId);
        Task UpdateAsync(Guid userId, AdminUpdateUserDto updateUserDto);
        Task UpdatePasswordAsync(Guid userId, string newPassword);
        Task UpdateUserEmailAsync(Guid userId, string newEmail);
        Task DeleteAsync(Guid userId);
        Task<bool> AlreadyDeletedAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(string email);
        Task<bool> AlreadyExistsExceptSpecificUserAsync(string email, Guid userId);
        Task<bool> SaveChangesAsync();
    }
}
