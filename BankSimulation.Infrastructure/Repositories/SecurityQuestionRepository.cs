using BankSimulation.Application.Dtos;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.DbContexts;
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

        public async Task AddSecurityQuestionAsync(SecurityQuestion securityQuestion)
        {
            await _context.SecurityQuestions
                .AddAsync(securityQuestion);
        }

        public async Task<string?> GetOnlyQuestionByUserIdAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AsNoTracking()
                .Where(sq => sq.UserId == userId)
                .Select(sq => sq.Question)
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetOnlyAnswerByUserIdAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AsNoTracking()
                .Where(sq => sq.UserId == userId)
                .Select(sq => sq.Answer)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> SecurityQuestionAlreadyExistsByUserIdAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AsNoTracking()
                .AnyAsync(sq => sq.UserId == userId);
        }

        public async Task UpdateSecurityQuestionByUserIdAsync(Guid userId, SecurityQuestionDto newSecurityQuestion)
        {
            await _context.SecurityQuestions
                .Where(sq => sq.UserId == userId)
                .ExecuteUpdateAsync(sq => sq
                .SetProperty(sq => sq.Question, newSecurityQuestion.Question)
                .SetProperty(sq => sq.Answer, newSecurityQuestion.Answer));
        }

        public async Task DeleteSecurityQuestionByUserIdAsync(Guid userId)
        {
            await _context.SecurityQuestions
                .Where(sq => sq.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
