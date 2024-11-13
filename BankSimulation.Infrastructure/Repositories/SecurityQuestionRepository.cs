using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Repositories;
using BankSimulation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankSimulation.Infrastructure.Repositories
{
    public class SecurityQuestionRepository : ISecurityQuestionRepository
    {
        private readonly AppDbContext _context;

        public SecurityQuestionRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(SecurityQuestion securityQuestion)
        {
            await _context.SecurityQuestions
                .AddAsync(securityQuestion);
        }

        public async Task<SecurityQuestion?> GetAsync(Guid userId, bool trackChanges)
        {
            var query = _context.SecurityQuestions.AsQueryable();

            if (!trackChanges) { query = query.AsNoTracking(); }

            return await query
                .SingleOrDefaultAsync(sq => sq.UserId == userId);
        }

        public async Task<string?> GetAnswerAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .Where(sq => sq.UserId == userId)
                .Select(sq => sq.Answer)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Guid userId, string newQuestion, string newAnswer)
        {
            await _context.SecurityQuestions
                .Where(sq => sq.UserId == userId)
                .ExecuteUpdateAsync(sq => sq
                .SetProperty(sq => sq.Question, newQuestion)
                .SetProperty(sq => sq.Answer, newAnswer));
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _context.SecurityQuestions
                .Where(sq => sq.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> AlreadyExistsAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AnyAsync(sq => sq.UserId == userId);
        }
    }
}
