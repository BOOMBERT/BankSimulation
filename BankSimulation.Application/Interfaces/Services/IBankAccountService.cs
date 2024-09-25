using BankSimulation.Application.Dtos;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IBankAccountService
    {
        Task<IEnumerable<BankAccountDto>> GetAllOwnBankAccountsAsync(string accessToken);
        Task<BankAccountDto> GetOwnBankAccountAsync(string accessToken, string bankAccountNumber);
    }
}
