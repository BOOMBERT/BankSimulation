using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface IMoneyOperationsService
    {
        Task<decimal> ExchangeCurrencyAsync(decimal amount, Currency currentCurrency, Currency requiredCurrency);
    }
}
