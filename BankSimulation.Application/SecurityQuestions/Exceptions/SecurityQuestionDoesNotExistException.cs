using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.SecurityQuestions.Exceptions
{
    public sealed class SecurityQuestionDoesNotExistException : CustomException
    {
        public SecurityQuestionDoesNotExistException(
            string errorContext,
            string title = "User Security Question Not Found",
            string details = "The specified user security question does not exist or could not be found.")
            : base(title, StatusCodes.Status404NotFound, details, errorContext) { }
    }
}
