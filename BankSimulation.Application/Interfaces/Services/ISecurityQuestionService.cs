namespace BankSimulation.Application.Interfaces.Services
{
    public interface ISecurityQuestionService
    {
        Task<string> GetOnlyQuestionByAccessTokenAsync(string accessToken);
        Task<bool> UpdateUserPasswordBySecurityQuestionAnswerAsync(string accessToken, string answer, string newPassword);
    }
}
