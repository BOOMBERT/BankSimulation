using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.SecurityQuestion
{
    public sealed class UserSecurityQuestionAlreadyExistException : CustomException
    {
        public UserSecurityQuestionAlreadyExistException(
            string errorContext,
            string title = "Security Question Already Exists",
            string details = "The security question for the specified user already exists.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
