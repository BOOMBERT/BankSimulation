using BankSimulation.Application.Dtos;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminBankAccountService
    {
        Task<bool> CreateUserBankAccountAsync(Guid userId, Currency bankAccountCurrency);
        Task<bool> DeleteUserBankAccountByNumberAsync(string bankAccountNumber);
        Task<BankAccountDto> GetUserBankAccountByNumberAsync(string bankAccountNumber);
        Task<IEnumerable<BankAccountDto>> GetAllBankAccountsByUserIdAsync(Guid userId);
    }
}
