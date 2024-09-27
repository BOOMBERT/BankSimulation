using BankSimulation.Application.Dtos.SecurityQuestion;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface ISecurityQuestionRepository
    {
        Task AddAsync(SecurityQuestion securityQuestion);
        Task<SecurityQuestionOutDto?> GetQuestionAsync(Guid userId);
        Task<string?> GetAnswerAsync(Guid userId);
        Task UpdateAsync(Guid userId, CreateSecurityQuestionDto newSecurityQuestion);
        Task DeleteAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(Guid userId);
    }
}
