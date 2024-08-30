using BankSimulation.Domain.Enums;

namespace BankSimulation.Application.Dtos
{
    public record BankAccountDto(Guid Id, string Number, Currency Currency, decimal Money, DateTime CreationDate, bool IsDeleted);
}