using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.SecurityQuestion
{
    public sealed class UserSecurityQuestionDoesNotExistException : CustomException
    {
        public UserSecurityQuestionDoesNotExistException(
            string errorContext,
            string title = "User Security Question Not Found",
            string details = "The specified user security question does not exist or could not be found.")
            : base(title, StatusCodes.Status409Conflict, details, errorContext) { }
    }
}
