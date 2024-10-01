using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Dtos.BankAccount
{
    public record BankAccountDto(string Number, Currency Currency, decimal Money, DateTime CreationDate, bool IsDeleted);
}