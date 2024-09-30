using BankSimulation.Application.Dtos.User;
using Microsoft.AspNetCore.JsonPatch;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(Guid userId, AdminUpdateUserDto updateUserDto);
        Task UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<AdminUpdateUserDto> patchDocument);
        Task DeleteUserAsync(Guid userId);
    }
}
