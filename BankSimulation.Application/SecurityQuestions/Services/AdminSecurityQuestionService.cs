using AutoMapper;
using BankSimulation.Application.Common.Utils;
using BankSimulation.Application.SecurityQuestions.Dtos;
using BankSimulation.Application.SecurityQuestions.Exceptions;
using BankSimulation.Application.SecurityQuestions.Interfaces;
using BankSimulation.Application.Users.Exceptions;
using BankSimulation.Domain.Entities;
using BankSimulation.Domain.Repositories;

namespace BankSimulation.Application.SecurityQuestions.Services
{
    public class AdminSecurityQuestionService : IAdminSecurityQuestionService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityQuestionRepository _securityQuestionRepository;

        public AdminSecurityQuestionService(IMapper mapper, IUserRepository userRepository, ISecurityQuestionRepository securityQuestionRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _securityQuestionRepository = securityQuestionRepository ?? throw new ArgumentNullException(nameof(securityQuestionRepository));
        }

        public async Task SetUserSecurityQuestionAsync(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate)
        {
            if (await UserHasSecurityQuestionAsync(userId))
            {
                throw new SecurityQuestionAlreadyExistException(userId.ToString());
            }

            var createdSecurityQuestion = new SecurityQuestion
            {
                Question = securityQuestionToCreate.Question,
                Answer = SecurityUtils.HashText(securityQuestionToCreate.Answer.ToUpper()),
                UserId = userId
            };

            await _securityQuestionRepository.AddAsync(createdSecurityQuestion);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<SecurityQuestionDto> GetSecurityQuestionByUserIdAsync(Guid userId)
        {
            if (!await _userRepository.AlreadyExistsAsync(userId))
            {
                throw new UserNotFoundException(userId.ToString());
            }

            return _mapper.Map<SecurityQuestionDto>(
                await _securityQuestionRepository.GetAsync(userId, false) 
                    ?? throw new SecurityQuestionDoesNotExistException(userId.ToString()));
        }

        public async Task ChangeSecurityQuestionByUserIdAsync(Guid userId, CreateSecurityQuestionDto securityQuestionToCreate)
        {
            if (!await UserHasSecurityQuestionAsync(userId))
            {
                throw new SecurityQuestionDoesNotExistException(userId.ToString());
            };

            await _securityQuestionRepository.UpdateAsync(
                userId, 
                securityQuestionToCreate.Question,
                SecurityUtils.HashText(securityQuestionToCreate.Answer.ToUpper()));

            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteSecurityQuestionByUserIdAsync(Guid userId)
        {
            if (!await UserHasSecurityQuestionAsync(userId))
            {
                throw new SecurityQuestionDoesNotExistException(userId.ToString());
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
