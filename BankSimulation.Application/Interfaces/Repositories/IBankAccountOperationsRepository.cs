using BankSimulation.Domain.Entities;

namespace BankSimulation.Application.Interfaces.Repositories
{
    public interface IBankAccountOperationsRepository
    {
        Task AddDepositAsync(Deposit deposit);
        Task AddWithdrawAsync(Withdraw withdraw);
        Task AddTransferAsync(Transfer transfer);
        Task<bool> SaveChangesAsync();
    }
}
