using AutoMapper;
using BankSimulation.Application.Dtos.User;
using BankSimulation.Application.Exceptions.SecurityQuestion;
using BankSimulation.Application.Exceptions.User;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Domain.Entities;

namespace BankSimulation.Infrastructure.Services
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

        public async Task<bool> SetUserSecurityQuestionAsync(string email, SecurityQuestionDto securityQuestionDto)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email) ?? throw new UserNotFoundException(email);

            if (await _securityQuestionRepository.SecurityQuestionAlreadyExistsByUserIdAsync(userId))
            {
                throw new UserSecurityQuestionAlreadyExistException(userId.ToString());
            }

            var securityQuestion = new SecurityQuestion
            {
                Question = securityQuestionDto.Question,
                Answer = BCrypt.Net.BCrypt.HashPassword(securityQuestionDto.Answer),
                UserId = userId
            };

            await _securityQuestionRepository.AddSecurityQuestionAsync(securityQuestion);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> ChangeSecurityQuestionByUserEmailAsync(string email, SecurityQuestionDto securityQuestionDto)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email) ?? throw new UserNotFoundException(email);

            var securityQuestionEntity = await _securityQuestionRepository.GetSecurityQuestionByUserIdAsync(userId)
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());

            _mapper.Map(securityQuestionDto, securityQuestionEntity);
            securityQuestionEntity.Answer = BCrypt.Net.BCrypt.HashPassword(securityQuestionEntity.Answer);

            return await _userRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSecurityQuestionByUserEmailAsync(string email)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email) ?? throw new UserNotFoundException(email);

            if (!await _securityQuestionRepository.SecurityQuestionAlreadyExistsByUserIdAsync(userId))
            {
                throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
            }
            await _securityQuestionRepository.DeleteSecurityQuestionByUserIdAsync(userId);
            return await _userRepository.SaveChangesAsync();
        }

        public async Task<string> GetSecurityQuestionByUserEmailAsync(string email)
        {
            var userId = await _userRepository.GetUserIdByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return await _securityQuestionRepository.GetOnlyQuestionByUserIdAsync(userId) 
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
        }
    }
}
