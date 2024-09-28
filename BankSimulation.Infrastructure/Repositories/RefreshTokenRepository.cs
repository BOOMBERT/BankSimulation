using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.DbContexts;
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

        public async Task<RefreshTokenDto?> GetAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.UserId == userId)
                .Select(rt => new RefreshTokenDto(rt.Token, rt.ExpirationDate))
                .SingleOrDefaultAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> AlreadyExistsAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .AsNoTracking()
                .AnyAsync(rt => rt.UserId == userId);
        }
    }
}
