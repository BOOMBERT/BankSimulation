using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Users.Exceptions
{
    public sealed class IncorrectCurrentEmailException : CustomException
    {
        public IncorrectCurrentEmailException(
            string errorContext,
            string title = "Incorrect Email",
            string details = "The provided current email is incorrect.")
            : base(title, StatusCodes.Status401Unauthorized, details, errorContext) { }
    }
}
