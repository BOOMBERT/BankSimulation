using BankSimulation.Application.Dtos.BankAccount;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminBankAccountService
    {
        Task<BankAccountDto> CreateUserBankAccountAsync(Guid userId, Currency bankAccountCurrency);
        Task<BankAccountDto> GetUserBankAccountAsync(Guid userId, string bankAccountNumber);
        Task<IEnumerable<BankAccountDto>> GetUserAllBankAccountsAsync(Guid userId);
        Task DeleteUserBankAccountAsync(Guid userId, string bankAccountNumber);
    }
}
