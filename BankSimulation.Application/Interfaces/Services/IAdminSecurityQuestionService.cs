using BankSimulation.Application.Dtos;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminSecurityQuestionService
    {
        Task<bool> SetUserSecurityQuestionAsync(Guid userId, SecurityQuestionDto securityQuestionDto);
        Task<bool> ChangeSecurityQuestionByUserIdAsync(Guid userId, SecurityQuestionDto securityQuestionDto);
        Task<bool> DeleteSecurityQuestionByUserIdAsync(Guid userId);
        Task<string> GetSecurityQuestionByUserIdAsync(Guid userId);
    }
}
