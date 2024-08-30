using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task AddBankAccountAsync(BankAccount bankAccount);
        Task<BankAccount?> GetBankAccountAsync(string bankAccountNumber);
        Task<IEnumerable<BankAccount>> GetUserAllBankAccountsAsync(Guid userId);
        Task<bool> BankAccountNumberAlreadyExistsAsync(string bankAccountNumber);
        Task<bool> BankAccountAlreadyDeletedAsync(string bankAccountNumber);
        Task DeleteBankAccountAsync(string bankAccountNumber);
    }
}
