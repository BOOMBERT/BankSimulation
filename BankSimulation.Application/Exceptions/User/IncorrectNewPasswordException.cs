using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Exceptions.User
{
    public sealed class IncorrectNewPasswordException : CustomException
    {
        public IncorrectNewPasswordException(
            string? errorContext = null,
            string title = "Incorrect Password",
            string details = "The new password cannot be the same as the current password.")
            : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
