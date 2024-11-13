using BankSimulation.Application.BankAccounts.Dtos;

namespace BankSimulation.Application.BankAccounts.Interfaces
{
    public interface IBankAccountService
    {
        Task<IEnumerable<BankAccountDto>> GetAllOwnBankAccountsAsync(string accessToken);
        Task<BankAccountDto> GetOwnBankAccountAsync(string accessToken, string bankAccountNumber);
    }
}
