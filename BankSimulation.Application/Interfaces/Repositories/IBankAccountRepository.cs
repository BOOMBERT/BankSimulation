using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task AddAsync(BankAccount bankAccount);
        Task<IEnumerable<BankAccount>> GetAsync(Guid userId);
        Task<BankAccount?> GetAsync(Guid userId, string bankAccountNumber, bool trackChanges = false);
        Task<Currency?> GetCurrencyAsync(Guid userId, string bankAccountNumber);
        Task<bool> AlreadyExistsAsync(string bankAccountNumber);
        Task<bool> AlreadyDeletedAsync(string bankAccountNumber);
        Task DepositMoneyAsync(decimal amount, string bankAccountNumber);
        Task DeleteAsync(Guid userId);
    }
}
