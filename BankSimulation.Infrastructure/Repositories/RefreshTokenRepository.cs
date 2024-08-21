using BankSimulation.Application.Dtos.Auth;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly UsersContext _context;

        public RefreshTokenRepository(UsersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens
                .AddAsync(refreshToken);
        }

        public async Task<RefreshTokenDto?> GetRefreshTokenByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.UserId == userId)
                .Select(rt => new RefreshTokenDto(rt.Token, rt.ExpirationDate))
                .SingleOrDefaultAsync();
        }

        public async Task DeleteRefreshTokenByUserIdAsync(Guid userId)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
