using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Enums;
using BankSimulation.Domain.Repositories;
using BankSimulation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(User user)
        {
            await _context.Users
                .AddAsync(user);
        }

        public async Task<User?> GetAsync(Guid userId, bool trackChanges)
        {
            var query = _context.Users.AsQueryable();

            if (!trackChanges) { query = query.AsNoTracking(); }

            return await query
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetAsync(string email, bool trackChanges)
        {
            var query = _context.Users.AsQueryable();

            if (!trackChanges) { query = query.AsNoTracking(); }

            return await query
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string?> GetPasswordAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Password)
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetEmailAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Email)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<AccessRole>> GetAccessRolesAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.AccessRoles)
                .SingleOrDefaultAsync() ?? Enumerable.Empty<AccessRole>();
        }

        public async Task UpdateAsync(Guid userId, string newFirstName, string newLastName, string newEmail, string newPassword)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u
                .SetProperty(x => x.FirstName, newFirstName)
                .SetProperty(x => x.LastName, newLastName)
                .SetProperty(x => x.Email, newEmail)
                .SetProperty(x => x.Password, newPassword));
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPassword)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u
                .SetProperty(x => x.Password, newPassword));
        }

        public async Task UpdateEmailAsync(Guid userId, string newEmail)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.Email, newEmail));
        }

        public async Task SoftDeleteAsync(Guid userId)
        {
            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.IsDeleted, true));
        }

        public async Task<bool> AlreadyDeletedAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.IsDeleted)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> AlreadyExistsAsync(Guid userId)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> AlreadyExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> AlreadyExistsExceptSpecificUserAsync(string email, Guid userIdToExcept)
        {
            return await _context.Users
                .Where(u => u.Email == email && u.Id != userIdToExcept)
                .AnyAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
