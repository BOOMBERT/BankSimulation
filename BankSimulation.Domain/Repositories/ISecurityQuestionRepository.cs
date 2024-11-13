using BankSimulation.Domain.Entities;

namespace BankSimulation.Domain.Repositories
{
    public interface ISecurityQuestionRepository
    {
        Task AddAsync(SecurityQuestion securityQuestion);
        Task<SecurityQuestion?> GetAsync(Guid userId, bool trackChanges);
        Task<string?> GetAnswerAsync(Guid userId);
        Task UpdateAsync(Guid userId, string newQuestion, string newAnswer);
        Task DeleteAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(Guid userId);
    }
}
