namespace BankSimulation.Application.SecurityQuestions.Dtos
{
    public sealed record ChangePasswordBySecurityQuestionDto(string Answer, string NewPassword);
}
