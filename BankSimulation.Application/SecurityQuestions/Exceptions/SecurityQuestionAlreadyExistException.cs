using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.SecurityQuestions.Exceptions
{
    public sealed class SecurityQuestionAlreadyExistException : CustomException
    {
        public SecurityQuestionAlreadyExistException(
            string errorContext,
            string title = "Security Question Already Exists",
            string details = "The security question for the specified user already exists.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
