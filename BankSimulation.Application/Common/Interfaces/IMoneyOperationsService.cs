using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Common.Interfaces
{
    public interface IMoneyOperationsService
    {
        Task<decimal> ExchangeCurrencyAsync(decimal amount, Currency currentCurrency, Currency requiredCurrency);
    }
}
