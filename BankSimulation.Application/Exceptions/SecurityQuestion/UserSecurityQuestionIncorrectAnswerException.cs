using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.SecurityQuestion
{
    public sealed class UserSecurityQuestionIncorrectAnswerException : CustomException
    {
        public UserSecurityQuestionIncorrectAnswerException(
           string errorContext,
           string title = "User Security Question Incorrect Answer",
           string details = "The provided answer for the user security question is incorrect.")
           : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
