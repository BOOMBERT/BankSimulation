using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface ISecurityQuestionRepository
    {
        Task AddSecurityQuestionAsync(SecurityQuestion securityQuestion);
        Task<bool> SecurityQuestionAlreadyExistsByUserIdAsync(Guid userId);
        Task<SecurityQuestion?> GetSecurityQuestionByUserIdAsync(Guid userId);
        Task<string?> GetOnlyQuestionByUserIdAsync(Guid userId);
        Task<string?> GetOnlyAnswerByUserIdAsync(Guid userId);
        Task DeleteSecurityQuestionByUserIdAsync(Guid userId);
    }
}
