using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Users.Exceptions
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
