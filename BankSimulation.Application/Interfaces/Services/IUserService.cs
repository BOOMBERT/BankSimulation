using BankSimulation.Application.Dtos.User;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user);
        Task<UserDto> GetUserViaAccessTokenAsync(string accessToken);
        Task UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword);
        Task UpdateUserEmailAsync(string accessToken, string currentEmail, string newEmail);
    }
}
