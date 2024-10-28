using BankSimulation.Application.Dtos.User;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user, AccessRole role = AccessRole.Customer);
        Task<UserDto> CreateAdminAsync();
        Task<UserDto> GetUserViaAccessTokenAsync(string accessToken);
        Task UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword);
        Task UpdateUserEmailAsync(string accessToken, string currentEmail, string newEmail);
    }
}
