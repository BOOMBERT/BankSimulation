using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly AppDbContext _context;

        public BankAccountRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddBankAccountAsync(BankAccount bankAccount)
        {
            await _context.BankAccounts
                .AddAsync(bankAccount);
        }

        public async Task<BankAccount?> GetBankAccountAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .AsNoTracking()
                .Where(ba => ba.Number == bankAccountNumber)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetUserAllBankAccountsAsync(Guid userId)
        {
            return await _context.BankAccounts
                .AsNoTracking()
                .Where(ba => ba.UserId == userId)
                .ToArrayAsync();
        }

        public async Task<bool> BankAccountNumberAlreadyExistsAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .AsNoTracking()
                .AnyAsync(ba => ba.Number == bankAccountNumber);
        }

        public async Task<bool> BankAccountAlreadyDeletedAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .AsNoTracking()
                .Where(ba => ba.Number == bankAccountNumber)
                .Select(ba => ba.IsDeleted)
                .SingleOrDefaultAsync();
        }

        public async Task DeleteBankAccountAsync(string bankAccountNumber)
        {
            await _context.BankAccounts
                .Where(ba => ba.Number == bankAccountNumber)
                .ExecuteUpdateAsync(ba => ba.SetProperty(x => x.IsDeleted, true));
        }
    }
}
