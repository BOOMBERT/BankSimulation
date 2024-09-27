using BankSimulation.Application.Dtos.SecurityQuestion;
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

        public async Task AddAsync(SecurityQuestion securityQuestion)
        {
            await _context.SecurityQuestions
                .AddAsync(securityQuestion);
        }

        public async Task<SecurityQuestionOutDto?> GetQuestionAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AsNoTracking()
                .Where(sq => sq.UserId == userId)
                .Select(sq => new SecurityQuestionOutDto(sq.Id, sq.Question))
                .SingleOrDefaultAsync();
        }

        public async Task<string?> GetAnswerAsync(Guid userId)
        {
            return await _context.SecurityQuestions
                .AsNoTracking()
                .Where(sq => sq.UserId == userId)
                .Select(sq => sq.Answer)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(Guid userId, CreateSecurityQuestionDto newSecurityQuestion)
        {
            await _context.SecurityQuestions
                .Where(sq => sq.UserId == userId)
                .ExecuteUpdateAsync(sq => sq
                .SetProperty(sq => sq.Question, newSecurityQuestion.Question)
                .SetProperty(sq => sq.Answer, newSecurityQuestion.Answer));
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
                .AsNoTracking()
                .AnyAsync(sq => sq.UserId == userId);
        }
    }
}
