using BankSimulation.Application.Dtos;
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

        public async Task<bool> SetUserSecurityQuestionAsync(Guid userId, SecurityQuestionDto securityQuestionDto)
        {
            await EnsureUserHasNoSecurityQuestionAsync(userId);

            var securityQuestion = new SecurityQuestion
            {
                Question = securityQuestionDto.Question,
                Answer = SecurityService.HashText(securityQuestionDto.Answer.ToUpper()),
                UserId = userId
            };

            await _securityQuestionRepository.AddSecurityQuestionAsync(securityQuestion);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> ChangeSecurityQuestionByUserIdAsync(Guid userId, SecurityQuestionDto securityQuestionDto)
        {
            await EnsureUserHasSecurityQuestionAsync(userId);
            
            var updatedSecurityQuestion = new SecurityQuestionDto(securityQuestionDto.Question, SecurityService.HashText(securityQuestionDto.Answer.ToUpper()));
            await _securityQuestionRepository.UpdateSecurityQuestionByUserIdAsync(userId, updatedSecurityQuestion);

            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSecurityQuestionByUserIdAsync(Guid userId)
        {
            await EnsureUserHasSecurityQuestionAsync(userId);
            await _securityQuestionRepository.DeleteSecurityQuestionByUserIdAsync(userId);

            return await _userRepository.SaveChangesAsync();
        }

        public async Task<string> GetSecurityQuestionByUserIdAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            return await _securityQuestionRepository.GetOnlyQuestionByUserIdAsync(userId)
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
        }

        private async Task EnsureUserHasNoSecurityQuestionAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }
            if (await _securityQuestionRepository.SecurityQuestionAlreadyExistsByUserIdAsync(userId))
            {
                throw new UserSecurityQuestionAlreadyExistException(userId.ToString());
            }
        }

        private async Task EnsureUserHasSecurityQuestionAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }
            if (!await _securityQuestionRepository.SecurityQuestionAlreadyExistsByUserIdAsync(userId))
            {
                throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
            }
        }
    }
}
