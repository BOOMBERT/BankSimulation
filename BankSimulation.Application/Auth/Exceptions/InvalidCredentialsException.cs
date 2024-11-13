using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Auth.Exceptions
{
    public sealed class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException(
            string errorContext,
            string title = "Invalid Credentials",
            string details = "The provided credentials are invalid. Please check your address email and password and try again.")
            : base(title, StatusCodes.Status401Unauthorized, details, errorContext) { }
    }
}
