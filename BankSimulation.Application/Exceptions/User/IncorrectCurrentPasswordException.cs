using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.User
{
    public sealed class IncorrectCurrentPasswordException : CustomException
    {
        public IncorrectCurrentPasswordException(
            string? errorContext = null,
            string title = "Incorrect Password",
            string details = "The provided current password is incorrect.")
            : base(title, StatusCodes.Status401Unauthorized, details, errorContext) { }
    }
}
