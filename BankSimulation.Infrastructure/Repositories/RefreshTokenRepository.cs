using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Repositories;
using BankSimulation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens
                .AddAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetAsync(Guid userId, bool trackChanges)
        {
            var query = _context.RefreshTokens.AsQueryable();

            if (!trackChanges) { query = query.AsNoTracking(); }

            return await query
                .Where(rt => rt.UserId == userId)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Guid userId, string newRefreshToken, DateTime newExpirationDate)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteUpdateAsync(rt => rt
                .SetProperty(x => x.Token, newRefreshToken)
                .SetProperty(x => x.ExpirationDate, newExpirationDate));
        }

        public async Task<bool> AlreadyExistsAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .AnyAsync(rt => rt.UserId == userId);
        }
    }
}
