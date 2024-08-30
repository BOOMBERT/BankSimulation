using BankSimulation.Application.Dtos;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface ISecurityQuestionRepository
    {
        Task AddSecurityQuestionAsync(SecurityQuestion securityQuestion);
        Task<string?> GetOnlyQuestionByUserIdAsync(Guid userId);
        Task<string?> GetOnlyAnswerByUserIdAsync(Guid userId);
        Task<bool> SecurityQuestionAlreadyExistsByUserIdAsync(Guid userId);
        Task UpdateSecurityQuestionByUserIdAsync(Guid userId, SecurityQuestionDto newSecurityQuestion);
        Task DeleteSecurityQuestionByUserIdAsync(Guid userId);
    }
}
