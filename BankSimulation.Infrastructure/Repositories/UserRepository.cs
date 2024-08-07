using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersContext _context;

        public UserRepository(UsersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailAlreadyExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task<RefreshToken?> GetUserRefreshTokenAsync(Guid userId)
        {
            return await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.UserId == userId);
        }

        public void DeleteUserRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
        }

        public async Task<IList<AccessRole>> GetUserAccessRolesAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.AccessRoles)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
