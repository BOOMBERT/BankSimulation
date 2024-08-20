using BankSimulation.Application.Dtos.User;
using Microsoft.AspNetCore.JsonPatch;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UpdateUserAsync(Guid userId, AdminUpdateUserDto updateUserDto);
        Task<bool> UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<AdminUpdateUserDto> patchDocument);
    }
}
