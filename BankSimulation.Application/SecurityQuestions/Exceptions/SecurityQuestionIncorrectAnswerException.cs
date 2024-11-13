using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.SecurityQuestions.Exceptions
{
    public sealed class SecurityQuestionIncorrectAnswerException : CustomException
    {
        public SecurityQuestionIncorrectAnswerException(
           string errorContext,
           string title = "User Security Question Incorrect Answer",
           string details = "The provided answer for the user security question is incorrect.")
           : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
