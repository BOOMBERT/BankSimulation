using BankSimulation.Application.SecurityQuestions.Dtos;

namespace BankSimulation.Application.SecurityQuestions.Interfaces
{
    public interface ISecurityQuestionService
    {
        Task<SecurityQuestionDto> GetOnlyQuestionByAccessTokenAsync(string accessToken);
        Task UpdateUserPasswordBySecurityQuestionAnswerAsync(string accessToken, string answer, string newPassword);
    }
}
