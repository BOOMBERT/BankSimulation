using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.BankAccounts.Dtos
{
    public sealed record BankAccountDto(string Number, Currency Currency, decimal Money, DateTime CreationDate, bool IsDeleted);
}