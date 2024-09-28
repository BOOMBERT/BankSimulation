using BankSimulation.Application.Dtos.User;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetUserDtoByIdAsync(Guid id);
        Task<UserDto?> GetUserDtoByEmailAsync(string email);
        Task<string?> GetUserPasswordByIdAsync(Guid userId);
        Task<AuthUserDto?> GetAuthDataAsync(string email);
        Task<IList<AccessRole>?> GetAccessRolesAsync(Guid userId);
        Task<string?> GetUserEmailByIdAsync(Guid userId);
        Task<bool> UserAlreadyDeletedByIdAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(string email);
        Task UpdateUserByIdAsync(Guid userId, AdminUpdateUserDto updateUserDto);
        Task UpdatePasswordAsync(Guid userId, string newPassword);
        Task UpdateUserEmailAsync(Guid userId, string newEmail);
        Task DeleteUserByIdAsync(Guid userId);
        Task<bool> SaveChangesAsync();
    }
}
