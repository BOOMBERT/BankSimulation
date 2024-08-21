using BankSimulation.Application.Dtos.User;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminSecurityQuestionService
    {
        Task<bool> SetUserSecurityQuestionAsync(string email, SecurityQuestionDto securityQuestionDto);
        Task<bool> ChangeSecurityQuestionByUserEmailAsync(string email, SecurityQuestionDto securityQuestionDto);
        Task<bool> DeleteSecurityQuestionByUserEmailAsync(string email);
        Task<string> GetSecurityQuestionByUserEmailAsync(string email);
    }
}
