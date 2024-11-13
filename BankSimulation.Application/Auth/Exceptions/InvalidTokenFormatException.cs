using BankSimulation.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BankSimulation.Application.Auth.Exceptions
{
    public sealed class InvalidTokenFormatException : CustomException
    {
        public InvalidTokenFormatException(
            string? errorContext = null,
            string title = "Invalid Token Format",
            string details = "The provided token format is invalid.")
            : base(title, StatusCodes.Status400BadRequest, details, errorContext) { }
    }
}
