using BankSimulation.Application.Dtos.User;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<bool> DeleteUserAsync(Guid id);
        Task<UserDto> GetUserViaAccessTokenAsync(string accessToken);
        Task<bool> UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword);
    }
}
