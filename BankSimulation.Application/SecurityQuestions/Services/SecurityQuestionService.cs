using AutoMapper;
using BankSimulation.Application.Auth.Interfaces;
using BankSimulation.Application.Common.Utils;
using BankSimulation.Application.SecurityQuestions.Dtos;
using BankSimulation.Application.SecurityQuestions.Exceptions;
using BankSimulation.Application.SecurityQuestions.Interfaces;
using BankSimulation.Domain.Repositories;

namespace BankSimulation.Application.SecurityQuestions.Services
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ISecurityQuestionRepository _securityQuestionRepository;
        private readonly IUserRepository _userRepository;

        public SecurityQuestionService(IMapper mapper, ITokenService tokenService, ISecurityQuestionRepository securityQuestionRepository, IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _securityQuestionRepository = securityQuestionRepository ?? throw new ArgumentNullException(nameof(securityQuestionRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<SecurityQuestionDto> GetOnlyQuestionByAccessTokenAsync(string accessToken)
        {
            var userId = _tokenService.GetUserIdFromJwt(accessToken);
            return _mapper.Map<SecurityQuestionDto>(
                await _securityQuestionRepository.GetAsync(userId, false) ?? throw new SecurityQuestionDoesNotExistException(userId.ToString()));
        }

        public async Task UpdateUserPasswordBySecurityQuestionAnswerAsync(string accessToken, string answer, string newPassword)
        {
            var userId = _tokenService.GetUserIdFromJwt(accessToken);
            var securityQuestionAnswer = await _securityQuestionRepository.GetAnswerAsync(userId)
                ?? throw new SecurityQuestionDoesNotExistException(userId.ToString());

            if (!SecurityUtils.VerifyHashedText(answer.ToUpper(), securityQuestionAnswer))
            {
                throw new SecurityQuestionIncorrectAnswerException(userId.ToString());
            }

            await _userRepository.UpdatePasswordAsync(userId, SecurityUtils.HashText(newPassword));
            await _userRepository.SaveChangesAsync();
        }
    }
}
