using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IAdminBankAccountOperationsService
    {
        Task DepositUserMoneyAsync(Guid userId, string bankAccountNumber, decimal amount, Currency currency);
    }
}
