using BankSimulation.Application.Dtos.SecurityQuestion;
using BankSimulation.Application.Exceptions.SecurityQuestion;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;
using BankSimulation.Infrastructure.Services.Utils;

namespace BankSimulation.Infrastructure.Services
{
    public class AdminSecurityQuestionService : IAdminSecurityQuestionService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityQuestionRepository _securityQuestionRepository;

        public AdminSecurityQuestionService(IUserRepository userRepository, ISecurityQuestionRepository securityQuestionRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _securityQuestionRepository = securityQuestionRepository ?? throw new ArgumentNullException(nameof(securityQuestionRepository));
        }

        public async Task<SecurityQuestionOutDto> SetUserSecurityQuestionAsync(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate)
        {
            if (await UserHasSecurityQuestionAsync(userId))
            {
                throw new UserSecurityQuestionAlreadyExistException(userId.ToString());
            }

            var createdSecurityQuestion = new SecurityQuestion
            {
                Question = securityQuestionToCreate.Question,
                Answer = SecurityService.HashText(securityQuestionToCreate.Answer.ToUpper()),
                UserId = userId
            };

            await _securityQuestionRepository.AddAsync(createdSecurityQuestion);
            await _userRepository.SaveChangesAsync();
            
            return new SecurityQuestionOutDto(createdSecurityQuestion.Id, createdSecurityQuestion.Question);
        }

        public async Task<SecurityQuestionOutDto> GetSecurityQuestionByUserIdAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            return await _securityQuestionRepository.GetQuestionAsync(userId)
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
        }

        public async Task ChangeSecurityQuestionByUserIdAsync(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate)
        {
            if (!await UserHasSecurityQuestionAsync(userId))
            {
                throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
            };
            
            var newSecurityQuestion = new CreateSecurityQuestionDto(
                securityQuestionToCreate.Question, 
                SecurityService.HashText(securityQuestionToCreate.Answer.ToUpper()));

            await _securityQuestionRepository.UpdateAsync(userId, newSecurityQuestion);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteSecurityQuestionByUserIdAsync(Guid userId)
        {
            if (!await UserHasSecurityQuestionAsync(userId))
            {
                throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
            }; 
            
            await _securityQuestionRepository.DeleteAsync(userId);
            await _userRepository.SaveChangesAsync();
        }

        private async Task<bool> UserHasSecurityQuestionAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }
            return await _securityQuestionRepository.AlreadyExistsAsync(userId);
        }
    }
}
