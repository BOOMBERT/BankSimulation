using BankSimulation.Application.Dtos.User;
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

        public async Task<bool> SetUserSecurityQuestionAsync(string email, SecurityQuestionDto securityQuestionDto)
        {
            var userId = await GetUserIdByEmailIfSecurityQuestionDoesNotExists(email);

            var securityQuestion = new SecurityQuestion
            {
                Question = securityQuestionDto.Question,
                Answer = SecurityService.HashText(securityQuestionDto.Answer.ToUpper()),
                UserId = userId
            };

            await _securityQuestionRepository.AddSecurityQuestionAsync(securityQuestion);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> ChangeSecurityQuestionByUserEmailAsync(string email, SecurityQuestionDto securityQuestionDto)
        {
            var userId = await GetUserIdByEmailIfSecurityQuestionExists(email);
            var updatedSecurityQuestion = new SecurityQuestionDto(securityQuestionDto.Question, SecurityService.HashText(securityQuestionDto.Answer.ToUpper()));
            await _securityQuestionRepository.UpdateSecurityQuestionByUserIdAsync(userId, updatedSecurityQuestion);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSecurityQuestionByUserEmailAsync(string email)
        {
            var userId = await GetUserIdByEmailIfSecurityQuestionExists(email);
            await _securityQuestionRepository.DeleteSecurityQuestionByUserIdAsync(userId);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<string> GetSecurityQuestionByUserEmailAsync(string email)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return await _securityQuestionRepository.GetOnlyQuestionByUserIdAsync(userId)
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
        }

        private async Task<Guid> GetUserIdByEmailIfSecurityQuestionExists(string email)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email) ?? throw new UserNotFoundException(email);

            if (!await _securityQuestionRepository.SecurityQuestionAlreadyExistsByUserIdAsync(userId))
            {
                throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
            }
            return userId;
        }

        private async Task<Guid> GetUserIdByEmailIfSecurityQuestionDoesNotExists(string email)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email) ?? throw new UserNotFoundException(email);

            if (await _securityQuestionRepository.SecurityQuestionAlreadyExistsByUserIdAsync(userId))
            {
                throw new UserSecurityQuestionAlreadyExistException(userId.ToString());
            }
            return userId;
        }
    }
}
