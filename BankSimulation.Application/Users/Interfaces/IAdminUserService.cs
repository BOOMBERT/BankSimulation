using BankSimulation.Application.Users.Dtos;
using BankSimulation.Domain.Enums;
using Microsoft.AspNetCore.JsonPatch;

namespace BankSimulation.Application.Users.Interfaces
{
    public interface IAdminUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user, AccessRole role = AccessRole.Customer);
        Task<UserDto> CreateAdminAsync();
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByBankAccountNumberAsync(string bankAccountNumber);
        Task UpdateUserAsync(Guid userId, CreateUserDto updateUserDto);
        Task UpdateUserPartiallyAsync(Guid userId, JsonPatchDocument<CreateUserDto> patchDocument);
        Task DeleteUserAsync(Guid userId);
    }
}
