using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Domain.Repositories
{
    public interface IBankAccountRepository
    {
        Task AddAsync(BankAccount bankAccount);
        Task<IEnumerable<BankAccount>> GetAsync(Guid userId, bool trackChanges);
        Task<BankAccount?> GetAsync(Guid userId, string bankAccountNumber, bool trackChanges);
        Task<Guid?> GetUserIdAsync(string bankAccountNumber);
        Task<Currency?> GetCurrencyAsync(string bankAccountNumber);
        Task<Currency?> GetCurrencyAsync(string bankAccountNumber, Guid userId);
        Task<decimal> GetBalanceAsync(string bankAccountNumber);
        Task DeleteAsync(Guid userId);
        Task<bool> AlreadyExistsAsync(string bankAccountNumber);
        Task<bool> AlreadyExistsAsync(string bankAccountNumber, Guid userId);
        Task<bool> AlreadyDeletedAsync(string bankAccountNumber);
        Task DepositMoneyAsync(decimal amount, string bankAccountNumber);
        Task WithdrawMoneyAsync(decimal amount, string bankAccountNumber);
    }
}
