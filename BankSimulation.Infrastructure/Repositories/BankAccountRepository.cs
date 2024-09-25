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

        public async Task AddAsync(BankAccount bankAccount)
        {
            await _context.BankAccounts
                .AddAsync(bankAccount);
        }

        public async Task<IEnumerable<BankAccount>> GetAsync(Guid userId)
        {
            return await _context.BankAccounts
                .AsNoTracking()
                .Where(ba => ba.UserId == userId)
                .ToArrayAsync();
        }

        public async Task<BankAccount?> GetAsync(Guid userId, string bankAccountNumber, bool trackChanges = false)
        {
            var query = _context.BankAccounts
                .Where(ba => ba.UserId == userId && ba.Number == bankAccountNumber);

            if (!trackChanges) { query = query.AsNoTracking(); }

            return await query.SingleOrDefaultAsync();
        }

        public async Task<bool> AlreadyExistsAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .AsNoTracking()
                .AnyAsync(ba => ba.Number == bankAccountNumber);
        }

        public async Task<bool> AlreadyDeletedAsync(string bankAccountNumber)
        {
            return await _context.BankAccounts
                .AsNoTracking()
                .Where(ba => ba.Number == bankAccountNumber)
                .Select(ba => ba.IsDeleted)
                .SingleOrDefaultAsync();
        }
    }
}
