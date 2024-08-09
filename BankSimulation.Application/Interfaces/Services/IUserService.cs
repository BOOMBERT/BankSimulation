using BankSimulation.Application.Dtos.User;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto user);
        Task<UserDto> GetUserAsync(Guid? id = null, string? email = null);
        Task<AuthUserDto?> GetUserAuthDataAsync(string email);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
