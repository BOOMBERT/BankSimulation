using BankSimulation.Application.Dtos.SecurityQuestion;
using BankSimulation.Application.Exceptions.SecurityQuestion;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;
using BankSimulation.Infrastructure.Services.Utils;

namespace BankSimulation.Infrastructure.Services
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly IUserAuthService _userAuthService;
        private readonly ISecurityQuestionRepository _securityQuestionRepository;
        private readonly IUserRepository _userRepository;

        public SecurityQuestionService(IUserAuthService userAuthService, ISecurityQuestionRepository securityQuestionRepository, IUserRepository userRepository)
        {
            _userAuthService = userAuthService ?? throw new ArgumentNullException(nameof(userAuthService));
            _securityQuestionRepository = securityQuestionRepository ?? throw new ArgumentNullException(nameof(securityQuestionRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<SecurityQuestionOutDto> GetOnlyQuestionByAccessTokenAsync(string accessToken)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            return await _securityQuestionRepository.GetQuestionAsync(userId)
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
        }

        public async Task UpdateUserPasswordBySecurityQuestionAnswerAsync(string accessToken, string answer, string newPassword)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            var securityQuestionAnswer = await _securityQuestionRepository.GetAnswerAsync(userId)
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());

            if (!SecurityService.VerifyHashedText(answer.ToUpper(), securityQuestionAnswer))
            {
                throw new UserSecurityQuestionIncorrectAnswerException(userId.ToString());
            }

            await _userRepository.UpdatePasswordAsync(userId, SecurityService.HashText(newPassword));
            await _userRepository.SaveChangesAsync();
        }
    }
}
