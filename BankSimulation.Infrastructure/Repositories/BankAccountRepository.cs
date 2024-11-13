using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Domain.Repositories;
using BankSimulation.Infrastructure.Persistence;
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

        public async Task AddAsync(BankAccount bankAccount)
        {
            await _context.BankAccounts
                .AddAsync(bankAccount);
        }

        public async Task<IEnumerable<BankAccount>> GetAsync(Guid userId, bool trackChanges)
        {
            var query = _context.BankAccounts.AsQueryable();

            if (!trackChanges) { query = query.AsNoTracking(); }

            return await query
                .Where(ba => ba.UserId == userId)
                .ToArrayAsync();
        }

        public async Task<BankAccount?> GetAsync(Guid userId, string bankAccountNumber, bool trackChanges)
        {
            var query = _context.BankAccounts.AsQueryable();

            if (!trackChanges) { query = query.AsNoTracking(); }

            return await query
                .Where(ba => ba.UserId == userId && ba.Number == bankAccountNumber)
                .SingleOrDefaultAsync();
        }

        public async Task<Guid?> GetUserIdAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
               .Where(ba => ba.Number == bankAccountNumber)
               .Select(ba => (Guid?)ba.UserId)
               .SingleOrDefaultAsync();
        }

        public async Task<Currency?> GetCurrencyAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .Where(ba => ba.Number == bankAccountNumber)
                .Select(ba => (Currency?)ba.Currency)
                .SingleOrDefaultAsync();
        }

        public async Task<Currency?> GetCurrencyAsync(string bankAccountNumber, Guid userId)
        {
            return await _context.BankAccounts
                .Where(ba => ba.Number == bankAccountNumber && ba.UserId == userId)
                .Select(ba => (Currency?)ba.Currency)
                .SingleOrDefaultAsync();
        }

        public async Task<decimal> GetBalanceAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .Where(ba => ba.Number == bankAccountNumber)
                .Select(ba => ba.Money)
                .SingleOrDefaultAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _context.BankAccounts
                .Where(ba => ba.UserId == userId && ba.IsDeleted == false)
                .ExecuteUpdateAsync(ba => ba
                .SetProperty(x => x.IsDeleted, true));
        }

        public async Task<bool> AlreadyExistsAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .AnyAsync(ba => ba.Number == bankAccountNumber);
        }

        public async Task<bool> AlreadyExistsAsync(string bankAccountNumber, Guid userId)
        {
            return await _context.BankAccounts
                .AnyAsync(ba => ba.Number == bankAccountNumber && ba.UserId == userId);
        }

        public async Task<bool> AlreadyDeletedAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .Where(ba => ba.Number == bankAccountNumber)
                .Select(ba => ba.IsDeleted)
                .SingleOrDefaultAsync();
        }

        public async Task DepositMoneyAsync(decimal amount, string bankAccountNumber)
        {
            await _context.BankAccounts
                .Where(ba => ba.Number == bankAccountNumber)
                .ExecuteUpdateAsync(ba => ba
                .SetProperty(x => x.Money, x => x.Money + amount));
        }

        public async Task WithdrawMoneyAsync(decimal amount, string bankAccountNumber)
        {
            await _context.BankAccounts
                .Where(ba => ba.Number == bankAccountNumber)
                .ExecuteUpdateAsync(ba => ba
                .SetProperty(x => x.Money, x => x.Money - amount));
        }
    }
}
