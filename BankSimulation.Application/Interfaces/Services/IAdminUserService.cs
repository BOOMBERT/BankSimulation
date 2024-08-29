using BankSimulation.Application.Dtos.User;
using Microsoft.AspNetCore.JsonPatch;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminUserService
    {
        Task<UserDto> GetUserAsync(Guid userId);
        Task<UserDto> GetUserAsync(string email);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> UpdateUserAsync(Guid userId, AdminUpdateUserDto updateUserDto);
        Task<bool> UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<AdminUpdateUserDto> patchDocument);
    }
}
