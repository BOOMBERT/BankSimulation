using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class SecurityQuestionRepository : ISecurityQuestionRepository
    {
        private readonly UsersContext _context;

        public SecurityQuestionRepository(UsersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddSecurityQuestionAsync(SecurityQuestion securityQuestion)
        {
            await _context.SecurityQuestions
                .AddAsync(securityQuestion);
        }

        public async Task<bool> SecurityQuestionAlreadyExistsByUserIdAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AsNoTracking()
                .AnyAsync(sq => sq.UserId == userId);
        }

        public async Task<SecurityQuestion?> GetSecurityQuestionByUserIdAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .SingleOrDefaultAsync(sq => sq.UserId == userId);
        }

        public async Task<string?> GetOnlyQuestionByUserIdAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AsNoTracking()
                .Where(sq => sq.UserId == userId)
                .Select(sq => sq.Question)
                .SingleOrDefaultAsync();
        }

        public async Task DeleteSecurityQuestionByUserIdAsync(Guid userId)
        {
            await _context.SecurityQuestions
                .Where(sq => sq.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
