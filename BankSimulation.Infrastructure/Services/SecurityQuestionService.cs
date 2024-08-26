using BankSimulation.Application.Exceptions.SecurityQuestion;
using BankSimulation.Application.Interfaces.Repositories;
using BankSimulation.Application.Interfaces.Services;

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

        public async Task<string> GetOnlySecurityQuestionByAccessTokenAsync(string accessToken)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            return await _securityQuestionRepository.GetOnlyQuestionByUserIdAsync(userId) 
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());
        }

        public async Task<bool> UpdateUserPasswordBySecurityQuestionAnswerAsync(string accessToken, string answer, string newPassword)
        {
            var userId = _userAuthService.GetUserIdFromJwt(accessToken);
            var securityQuestionAnswer = await _securityQuestionRepository.GetOnlyAnswerByUserIdAsync(userId) 
                ?? throw new UserSecurityQuestionDoesNotExistException(userId.ToString());

            if (!SecurityService.VerifyHashedText(answer.ToUpper(), securityQuestionAnswer))
            {
                throw new UserSecurityQuestionIncorrectAnswerException(userId.ToString());
            }
            newPassword = SecurityService.HashText(newPassword);

            await _userRepository.UpdateUserPasswordAsync(userId, newPassword);
            return await _userRepository.SaveChangesAsync();
        }
    }
}
