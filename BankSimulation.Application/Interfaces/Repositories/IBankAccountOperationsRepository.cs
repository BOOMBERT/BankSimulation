using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IBankAccountOperationsRepository
    {
        Task AddDepositAsync(Deposit deposit);
    }
}
