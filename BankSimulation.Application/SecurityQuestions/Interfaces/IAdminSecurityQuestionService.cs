using BankSimulation.Application.SecurityQuestions.Dtos;

namespace BankSimulation.Application.SecurityQuestions.Interfaces
{
    public interface IAdminSecurityQuestionService
    {
        Task SetUserSecurityQuestionAsync(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate);
        Task<SecurityQuestionDto> GetSecurityQuestionByUserIdAsync(Guid userId);
        Task ChangeSecurityQuestionByUserIdAsync(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate);
        Task DeleteSecurityQuestionByUserIdAsync(Guid userId);
    }
}
