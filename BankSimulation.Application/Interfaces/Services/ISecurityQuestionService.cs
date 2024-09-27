using BankSimulation.Application.Dtos.SecurityQuestion;

namespace BankSimulation.Application.Interfaces.Services
{
    public interface ISecurityQuestionService
    {
        Task<SecurityQuestionOutDto> GetOnlyQuestionByAccessTokenAsync(string accessToken);
        Task UpdateUserPasswordBySecurityQuestionAnswerAsync(string accessToken, string answer, string newPassword);
    }
}
