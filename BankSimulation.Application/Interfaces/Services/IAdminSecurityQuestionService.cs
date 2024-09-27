using BankSimulation.Application.Dtos.SecurityQuestion;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminSecurityQuestionService
    {
        Task<SecurityQuestionOutDto> SetUserSecurityQuestionAsync(Guid userId, CreateSecurityQuestionDto createSecurityQuestionDto);
        Task<SecurityQuestionOutDto> GetSecurityQuestionByUserIdAsync(Guid userId);
        Task ChangeSecurityQuestionByUserIdAsync(Guid userId, CreateSecurityQuestionDto createSecurityQuestionDto);
        Task DeleteSecurityQuestionByUserIdAsync(Guid userId);
    }
}
