using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.BankAccounts.Dtos
{
    public sealed record BankAccountDto(string Number, Currency Currency, decimal Balance, DateTime CreationDate, bool IsDeleted);
}