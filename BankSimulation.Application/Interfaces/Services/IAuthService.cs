using BankSimulation.Application.Dtos.User;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateAccessToken(AuthUserDto user);
        bool VerifyUserPassword(string plainPassword, string passwordHash);
    }
}
