using BankSimulation.Application.Dtos;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminBankAccountService
    {
        Task<BankAccountDto> CreateUserBankAccountAsync(Guid userId, Currency bankAccountCurrency);
        Task DeleteUserBankAccountAsync(Guid userId, string bankAccountNumber);
        Task<BankAccountDto> GetUserBankAccountAsync(Guid userId, string bankAccountNumber);
        Task<IEnumerable<BankAccountDto>> GetUserAllBankAccountsAsync(Guid userId);
    }
}
