namespace BankSimulation.Application.Dtos.User
{
    public sealed record ChangePasswordBySecurityQuestionDto(string Answer, string NewPassword);
}
