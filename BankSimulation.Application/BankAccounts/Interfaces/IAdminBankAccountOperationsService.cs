using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.BankAccounts.Interfaces
{
    public interface IAdminBankAccountOperationsService
    {
        Task DepositUserMoneyAsync(Guid userId, string bankAccountNumber, decimal amount, Currency currency);
        Task WithdrawUserMoneyAsync(Guid userId, string bankAccountNumber, decimal amount, Currency currency);
    }
}
