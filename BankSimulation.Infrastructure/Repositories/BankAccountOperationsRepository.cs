using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.DbContexts;

namespace BankSimulation.Infrastructure.Repositories
{
    public class BankAccountOperationsRepository : IBankAccountOperationsRepository
    {
        private readonly AppDbContext _context;

        public BankAccountOperationsRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddDepositAsync(Deposit deposit)
        {
            await _context.Deposits
                .AddAsync(deposit);
        }

        public async Task AddWithdrawAsync(Withdraw withdraw)
        {
            await _context.Withdraws
                .AddAsync(withdraw);
        }

        public async Task AddTransferAsync(Transfer transfer)
        {
            await _context.Transfers
                .AddAsync(transfer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
