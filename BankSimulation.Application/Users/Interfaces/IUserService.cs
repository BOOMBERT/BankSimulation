using BankSimulation.Application.Users.Dtos;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserViaAccessTokenAsync(string accessToken);
        Task UpdateUserPasswordAsync(string accessToken, string currentPassword, string newPassword);
        Task UpdateUserEmailAsync(string accessToken, string currentEmail, string newEmail);
    }
}
