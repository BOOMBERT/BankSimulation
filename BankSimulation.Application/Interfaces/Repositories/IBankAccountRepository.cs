using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task AddAsync(BankAccount bankAccount);
        Task<IEnumerable<BankAccount>> GetAsync(Guid userId);
        Task<BankAccount?> GetAsync(Guid userId, string bankAccountNumber, bool trackChanges = false);
        Task<bool> AlreadyExistsAsync(string bankAccountNumber);
        Task<bool> AlreadyDeletedAsync(string bankAccountNumber);
    }
}
